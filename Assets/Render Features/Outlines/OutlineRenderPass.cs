﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ProfilingScope = UnityEngine.Rendering.ProfilingScope;

namespace RenderFeatures.Outlines
{
    public class OutlineRenderPass : ScriptableRenderPass
    {
        private readonly OutlineSettings m_Settings;

        private readonly Material m_NormalsMaterial;
        private readonly Material m_OutlineMaterial;

        private readonly FilteringSettings m_FilteringSettings;
        private readonly List<ShaderTagId> m_ShaderTagIds = new();

        private RendererList m_RendererList;

        /// <summary>
        /// Used as a render target for drawing color information.
        /// </summary>
        private RTHandle m_NormalsTextureHandle;

        /// <summary>
        /// Used as a temporary texture target when blitting.
        /// </summary>
        private RTHandle m_TempColorTextureHandle;

        // Ids of shader properties to use when updating.
        private static readonly int OutlineScaleId = Shader.PropertyToID("_OutlineScale");
        private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");
        private static readonly int RobertsCrossMultiplierId = Shader.PropertyToID("_RobertsCrossMultiplier");
        private static readonly int DepthThresholdId = Shader.PropertyToID("_DepthThreshold");
        private static readonly int NormalThresholdId = Shader.PropertyToID("_NormalThreshold");
        private static readonly int SteepAngleThresholdId = Shader.PropertyToID("_SteepAngleThreshold");
        private static readonly int SteepAngleMultiplierId = Shader.PropertyToID("_SteepAngleMultiplier");

        public OutlineRenderPass(OutlineSettings settings)
        {
            m_Settings = settings;
            m_NormalsMaterial = new Material(Shader.Find("Hidden/ViewSpaceNormals"));
            m_OutlineMaterial = new Material(Shader.Find("Hidden/Outlines"));

            // Set the stage of when this feature will get rendered.
            renderPassEvent = m_Settings.RenderPassEvent;

            // Make sure we use our layers in the filtering settings.
            var renderLayer = (uint)1 << m_Settings.RenderLayerMask;
            m_FilteringSettings =
                new FilteringSettings(RenderQueueRange.opaque, m_Settings.LayerMask, renderLayer);

            // Use default shader tags.
            m_ShaderTagIds.Add(new ShaderTagId("SRPDefaultUnlit"));
            m_ShaderTagIds.Add(new ShaderTagId("UniversalForward"));
            m_ShaderTagIds.Add(new ShaderTagId("UniversalForwardOnly"));
        }

        /// <summary>
        /// Updates shader properties based on settings.
        /// </summary>
        private void UpdateSettings()
        {
            if (m_OutlineMaterial == null)
                return;

            m_OutlineMaterial.SetFloat(OutlineScaleId, m_Settings.OutlineScale);
            m_OutlineMaterial.SetColor(OutlineColorId, m_Settings.OutlineColor);
            m_OutlineMaterial.SetFloat(RobertsCrossMultiplierId, m_Settings.RobertsCrossMultiplier);
            m_OutlineMaterial.SetFloat(DepthThresholdId, m_Settings.DepthThreshold);
            m_OutlineMaterial.SetFloat(NormalThresholdId, m_Settings.NormalThreshold);
            m_OutlineMaterial.SetFloat(SteepAngleThresholdId, m_Settings.SteepAngleThreshold);
            m_OutlineMaterial.SetFloat(SteepAngleMultiplierId, m_Settings.SteepAngleMultiplier);
        }

        [System.Obsolete]
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Set up texture descriptors for color and depth
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.colorFormat = m_Settings.RenderTextureFormat;
            descriptor.depthBufferBits = (int)DepthBits.None;

            // Reallocate render textures if needed
            RenderingUtils.ReAllocateIfNeeded(ref m_NormalsTextureHandle, descriptor, name: "_NormalsTexture",
                wrapMode: TextureWrapMode.Clamp);
            RenderingUtils.ReAllocateIfNeeded(ref m_TempColorTextureHandle, descriptor, name: "_TempColorTexture",
                wrapMode: TextureWrapMode.Clamp);

            var cameraTargetDepth = renderingData.cameraData.renderer.cameraDepthTargetHandle;
            ConfigureTarget(m_NormalsTextureHandle, cameraTargetDepth);

            // Make sure the color is transparent.
            m_Settings.BackgroundColor.a = 0;
            ConfigureClear(ClearFlag.Color, m_Settings.BackgroundColor);
        }

        private void InitRendererLists(ref RenderingData renderingData, ScriptableRenderContext context)
        {
            var sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;

            var drawingSettings = CreateDrawingSettings(m_ShaderTagIds, ref renderingData, sortingCriteria);
            drawingSettings.overrideMaterial = m_NormalsMaterial;
            drawingSettings.overrideMaterialPassIndex = 0;

            var param = new RendererListParams(renderingData.cullResults, drawingSettings, m_FilteringSettings);
            m_RendererList = context.CreateRendererList(ref param);
        }

        [System.Obsolete]
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (m_NormalsMaterial == null || m_OutlineMaterial == null)
                return;

            var cmd = CommandBufferPool.Get();
            var cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

            UpdateSettings();

            using (new ProfilingScope(cmd, new ProfilingSampler("OutlinePass")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                // Make sure we clear the depth buffer.
                cmd.ClearRenderTarget(RTClearFlags.All, m_Settings.BackgroundColor,1,0);

                // Initialize and draw all renderers.
                InitRendererLists(ref renderingData, context);
                cmd.DrawRendererList(m_RendererList);

                // Pass our filter texture to shaders as a global texture reference.
                // Obtain this in a shader graph as a Texture2D with exposed un-ticked
                // and reference _NormalsTexture.
                cmd.SetGlobalTexture(Shader.PropertyToID(m_NormalsTextureHandle.name), m_NormalsTextureHandle);

                // For some reasons these rt are null for a frame when selecting in scene view so we need to check for null.
                if (cameraTargetHandle.rt != null && m_TempColorTextureHandle.rt != null)
                {
                    Blitter.BlitCameraTexture(cmd, cameraTargetHandle, m_TempColorTextureHandle, m_OutlineMaterial, 0);
                    Blitter.BlitCameraTexture(cmd, m_TempColorTextureHandle, cameraTargetHandle);
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// <summary>
        /// Releases all used resources. Called by the feature.
        /// </summary>
        public void Dispose()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                Object.Destroy(m_NormalsMaterial);
                Object.Destroy(m_OutlineMaterial);
            }
            else
            {
                Object.DestroyImmediate(m_NormalsMaterial);
                Object.DestroyImmediate(m_NormalsMaterial);
            }
#else
            Object.Destroy(m_NormalsMaterial);
            Object.Destroy(m_OutlineMaterial);
#endif

            m_NormalsTextureHandle?.Release();
            m_TempColorTextureHandle?.Release();
        }
    }
}