using System;
using System.Collections;
using UnityEngine;

public class BuildingUpgradable : MonoBehaviour
{
    [SerializeField] private BuildingUpgradesSO buildingUpgrades;
    private GameObject currentBuildingVisual;

    [SerializeField] private bool incomePerRound;
    [SerializeField] private string buildingName;
    [SerializeField] private string buildingDescription;
   
    private float miningCooldown;
    private int earnings;
    private int currentUpgrade;

    private int currentMoneyInBuilding;

    [SerializeField] private GameObject getResourcesButton;

    public event Action OnWorkingStart;
    public event Action OnMiningStop;


    private void Start()
    {
        miningCooldown = buildingUpgrades.buildingUpgrades[currentUpgrade].cooldown;
        earnings = buildingUpgrades.buildingUpgrades[currentUpgrade].earnings;

        StartCoroutine(Mine());
    }

    IEnumerator Mine()
    {
        while (true)
        {
            yield return new WaitForSeconds(miningCooldown);

            if (AISpawner.Instance.WaveInProgress())
            {
                if (incomePerRound)
                {
                    currentMoneyInBuilding = earnings;
                    OnWorkingStart?.Invoke();
                }
                else
                {
                    currentMoneyInBuilding += earnings;
                    OnWorkingStart?.Invoke();
                }
            }
            else
            {
                OnMiningStop?.Invoke();
                if (currentMoneyInBuilding > 0)
                {
                    getResourcesButton.SetActive(true);
                }
                else
                {
                    getResourcesButton.SetActive(false);
                }
            }

            

        }
    }

    public void GetResources()
    {
        ResourcesManager.Instance.GetGold(currentMoneyInBuilding);
        currentMoneyInBuilding = 0;
    }

    public void Upgrade()
    {
        currentUpgrade++;
        ResetMining();
    }

    private void ResetMining()
    {
        StopAllCoroutines();

        miningCooldown = buildingUpgrades.buildingUpgrades[currentUpgrade].cooldown;
        earnings = buildingUpgrades.buildingUpgrades[currentUpgrade].earnings;

        Destroy(currentBuildingVisual);
        currentBuildingVisual = Instantiate(buildingUpgrades.buildingUpgrades[currentUpgrade].newPrefab, transform);

        StartCoroutine(Mine());
    }

    public string GetBuildingName()
    {
        return buildingName;
    }

    public string GetUpgradeCost()
    {
        return buildingUpgrades.buildingUpgrades[currentUpgrade].upgradeCost.ToString();
    }

    public string GetCurrentEarnings()
    {
        return earnings.ToString();
    }

    public string GetNextLevelEarnings()
    {
        return buildingUpgrades.buildingUpgrades[currentUpgrade].earnings.ToString();
    }

    public int GetCurrentLevel()
    {
        return currentUpgrade;
    }

}
