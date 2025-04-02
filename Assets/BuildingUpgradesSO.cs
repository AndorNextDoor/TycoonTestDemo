using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building-Upgrades", menuName = "Building Upgrades/New building upgrade")]
public class BuildingUpgradesSO : ScriptableObject
{
    public List<BuildingUpgrade> buildingUpgrades;
}

[Serializable]
public class BuildingUpgrade
{
    public float cooldown;
    public int earnings;
    public GameObject newPrefab;
    public int upgradeCost;
}
