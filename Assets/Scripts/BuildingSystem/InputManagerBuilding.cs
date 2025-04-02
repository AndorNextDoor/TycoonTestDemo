using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerBuilding : MonoBehaviour
{
    public static InputManagerBuilding Instance;

    private Camera mainCamera;
    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayer;

    public event Action OnClicked, OnExit;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, 1000f, placementLayer))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    
}
