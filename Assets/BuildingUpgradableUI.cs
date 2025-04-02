using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUpgradableUI : MonoBehaviour
{
    public static BuildingUpgradableUI Instance;

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI earningsText;    
    
    [SerializeField] private TextMeshProUGUI buildingNextLevelNameText;
    [SerializeField] private TextMeshProUGUI levelNextLevelText;
    [SerializeField] private TextMeshProUGUI earningsNextLevelText;

    [SerializeField] private TextMeshProUGUI upgradeCostText;

    private void Awake()
    {
        Instance = this;
        uiPanel.SetActive(false); // Hide UI at the start
    }

    public void ShowBuildingInfo(BuildingUpgradable building)
    {
        uiPanel.SetActive(true);
        buildingNameText.text   = building.GetBuildingName();
        levelText.text          = "Level: " + building.GetCurrentLevel();
        earningsText.text       = "Earnings: " + building.GetCurrentEarnings();
        upgradeCostText.text    = "Upgrade Cost: " + building.GetUpgradeCost();

        levelNextLevelText.text     = building.GetNextLevelEarnings();
        earningsNextLevelText.text = (building.GetCurrentLevel() + 1).ToString();
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
    }
}


