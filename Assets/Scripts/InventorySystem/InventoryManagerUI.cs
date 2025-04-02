using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private GameObject inventoryObjectContainerPrefab;

    [SerializeField] private Transform inventoryObjectContainerParent;

    public void GenerateInventoryMenu()
    {
        foreach (Transform child in inventoryObjectContainerParent)
        {
            Destroy(child.gameObject);
        }

        List<InventoryObject> playersInventory = InventoryManager.Instance.GetInventory();
        foreach (InventoryObject inventoryObject in playersInventory)
        {
            GameObject newButton = Instantiate(inventoryObjectContainerPrefab, inventoryObjectContainerParent);

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryObject.objectData.Name + " x" + inventoryObject.quantity;
            newButton.GetComponentInChildren<Image>().sprite = inventoryObject.objectData.icon;

            Button button = newButton.GetComponentInChildren<Button>();
            if (inventoryObject.objectData.Placable)
            {
                button.onClick.AddListener(() => PlacementSystem.Instance.SelectObjectToPlace(inventoryObject.objectData.ID));
                button.onClick.AddListener(() => HideInventoryMenu());
            }
            else
            {
                // TO DO: Logic for items that player can use

            }
        }
    }

    public void HideInventoryMenu()
    {
        inventoryMenu.SetActive(false);
    }

}
