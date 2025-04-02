using System;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance;

    private DatabaseObject currentObject;

    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private Grid grid;
    private InputManagerBuilding inputManager;

    private GameObject selectedObject;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0.05f, 0.5f);

    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private LayerMask buildingGhostLayer;

    private void Awake()
    {
        Instance = this;
        gridVisualization.SetActive(false);
    }

    private void Start()
    {
        inputManager = InputManagerBuilding.Instance;

        ExitPlacement();
    }

    private void ExitPlacement()
    {
        currentObject = null;
        Destroy(selectedObject);

        HideGridAndIndicators();
    }

    public void SelectObjectToPlace(int ID)
    {
        gridVisualization.SetActive(true);

        currentObject = ObjectsDatabase.Instance.FindObjectByID(ID);
        selectedObject = Instantiate(currentObject.Prefab);

        DisableAllScriptsForSelectedObject();

        if (currentObject == null)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        ShowGridAndIndicators();
    }

    private void PlaceObject()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = InputManagerBuilding.Instance.GetSelectedMapPosition();
        currentObject = ObjectsDatabase.Instance.FindObjectByIndex(currentObject.ID);
        GameObject newStructure = Instantiate(currentObject.Prefab);


        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        newStructure.transform.position = grid.CellToWorld(gridPosition) + new Vector3(offset.x * currentObject.Size.x, offset.y, offset.z);

        if (newStructure.TryGetComponent<AI>(out AI ai))
        {
            ai.OnDeathGameObject += SaveManager.Instance.RemoveSavableGuardian;
            SaveManager.Instance.AddSavableGuardian(currentObject.Prefab, newStructure.transform.position);
        }
        InventoryManager.Instance.RemoveItemFromInventory(currentObject);

        ExitPlacement();
    }

    private void Update()
    {
        if (inputManager == null)
            return;

        if(currentObject == null)
        {
            return;
        }

        Vector3 mousePosition = InputManagerBuilding.Instance.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;

        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        //TO DO: If object is selected show object ghost instead of an indicator
        selectedObject.transform.position = cellIndicator.transform.position + new Vector3(offset.x * currentObject.Size.x, offset.y, offset.z);

    }

    private void ShowGridAndIndicators()
    {
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceObject;
        inputManager.OnExit += ExitPlacement;
    }

    private void HideGridAndIndicators()
    {
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceObject;
        inputManager.OnExit -= ExitPlacement;
    }

    private void DisableAllScriptsForSelectedObject()
    {
        MonoBehaviour[] scripts = selectedObject.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        Collider[] colliders = selectedObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        selectedObject.GetComponent<Collider>().enabled = false;

        selectedObject.layer = buildingGhostLayer.value;
    }

}
