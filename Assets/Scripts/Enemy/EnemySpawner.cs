using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public static AISpawner Instance;

    [Header("Spawn Settings")]
    public Transform spawnPoint;  // The main spawn position
    public int numCircles = 3;    // Number of half-circles
    public float radiusStep = 3f; // Distance between each circle
    public int spawnCountPerCircle = 5; // Enemies per circle

    [SerializeField] private EnemyWave[] enemyWaves;
    private int currentWaveIndex = 0;
    private bool IsWaveInProgress = false;

    private int currentWaveEnemiesAmount = -1;

    [SerializeField] private GameObject readyButton;


    private void Awake()
    {
        Instance = this;
    }

    public void StartWave()
    {
        if (IsWaveInProgress) return;

        readyButton.SetActive(false);
        if (currentWaveIndex >= enemyWaves.Length) return;

        GenerateSpawnPoints();
        currentWaveIndex++;
        IsWaveInProgress = true;
    }

    private void GenerateSpawnPoints()
    {
        EnemyWave wave = enemyWaves[currentWaveIndex]; // Get the current wave

        int totalEnemies = 0;
        foreach (Wave enemyGroup in wave.waves)
        {
            totalEnemies += enemyGroup.quantity;
        }

        List<Vector3> spawnPositions = GetSpawnPositions(totalEnemies);

        int spawnIndex = 0;
        foreach (Wave enemyGroup in wave.waves)
        {
            string enemyType = enemyGroup.enemyPrefab.GetComponentInChildren<AI>().AIName;

            for (int i = 0; i < enemyGroup.quantity; i++)
            {
                if (spawnIndex >= spawnPositions.Count) break; // Safety check

                GameObject enemy = AIPool.Instance.GetAI(enemyType);
                enemy.transform.position = spawnPositions[spawnIndex];
                enemy.transform.rotation = Quaternion.identity;

                enemy.GetComponent<AI>().OnDeath += OnEnemyDeath;
                spawnIndex++;
            }
        }
    }

    private List<Vector3> GetSpawnPositions(int totalEnemies)
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        int enemiesPerCircle = Mathf.Max(1, totalEnemies / numCircles);
        int spawnedEnemies = 0;

        for (int circle = 0; circle < numCircles; circle++)
        {
            float currentRadius = (circle + 1) * radiusStep;
            float startAngle = -90f;
            float endAngle = 90f;
            int enemiesInThisCircle = Mathf.Min(spawnCountPerCircle, totalEnemies - spawnedEnemies);

            for (int i = 0; i < enemiesInThisCircle; i++)
            {
                float angle = Mathf.Lerp(startAngle, endAngle, (float)i / (enemiesInThisCircle - 1));
                float radian = angle * Mathf.Deg2Rad;

                Vector3 spawnPos = spawnPoint.position + new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * currentRadius;
                spawnPositions.Add(spawnPos);
                spawnedEnemies++;

                if (spawnedEnemies >= totalEnemies)
                    return spawnPositions; // Stop if we have enough positions
            }
        }

        return spawnPositions;
    }

    public void OnEnemyDeath()
    {
        currentWaveEnemiesAmount--;
        if(currentWaveEnemiesAmount <= 0)
        {
            IsWaveInProgress = false;
            readyButton.SetActive(true);
        }
    }

    public bool WaveInProgress()
    {
        return IsWaveInProgress;
    }
}
