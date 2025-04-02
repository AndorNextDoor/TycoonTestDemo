using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private Transform shopContainerParent;
    [SerializeField] private GameObject shopContainerPrefab;


    public void GenerateShopMenu()
    {
        foreach (Transform child in shopContainerParent)
        {
            Destroy(child.gameObject);
        }

        foreach (DatabaseObject _object in ObjectsDatabase.Instance.GetObjectsDatabase().objectsData)
        {
            if (!ProgressionManager.Instance.IsEnoughLevel(_object.RequiredLevel))
            {
                continue;
            }

            GameObject newButton = Instantiate(shopContainerPrefab, shopContainerParent);


            Button button = newButton.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => BuyItem(_object));

            newButton.GetComponentInChildren<Image>().sprite = _object.icon;
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = _object.Cost.ToString();
        }
    }

    private void BuyItem(DatabaseObject _object)
    {
        if (ResourcesManager.Instance.HaveEnoughToBuy(_object.Cost))
        {
            ResourcesManager.Instance.SpendGold(_object.Cost);
            Debug.Log("Item bought");
            InventoryManager.Instance.AddItemToInventory(_object);
        }
        else
        {
            return;
        }
    }

}
