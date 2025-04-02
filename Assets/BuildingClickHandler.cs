using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingClickHandler : MonoBehaviour
{
    private BuildingUpgradable building;

    private void Start()
    {
        building = GetComponent<BuildingUpgradable>();
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Prevent clicking through UI
        {
            BuildingUpgradableUI.Instance.ShowBuildingInfo(building);
        }
    }
}
