using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/New wave")]
public class EnemyWave : ScriptableObject
{
    public List<Wave> waves;
}
[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int quantity;
}
