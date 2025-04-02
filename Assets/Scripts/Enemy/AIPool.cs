using System.Collections.Generic;
using UnityEngine;

public class AIPool : MonoBehaviour
{
    public static AIPool Instance;
    public List<PoolData> AITypes; 
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Initialize pools for each AI type
        foreach (PoolData pool in AITypes)
        {
            string AIName = null;
            Queue<GameObject> AIQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject AI = Instantiate(pool.AI);
                AI.SetActive(false);
                AI AIScript = AI.GetComponentInChildren<AI>();
                AIScript.enabled = false;
                AIName = pool.AI.GetComponentInChildren<AI>().AIName;
                AIScript.AIName = AIName;
                AIQueue.Enqueue(AI);
            }

            poolDictionary.Add(AIName, AIQueue);
        }
    }
    public GameObject GetAI(string AIType)
    {
        if (!poolDictionary.ContainsKey(AIType))
        {
            Debug.LogError($"AI type '{AIType}' not found in pool!");
            return null;
        }

        GameObject AI;

        if (poolDictionary[AIType].Count > 0)
        {
            AI = poolDictionary[AIType].Dequeue();
        }
        else
        {
            AI = Instantiate(AITypes.Find(p => p.AI.GetComponentInChildren<AI>().AIName == AIType).AI);
            AI.GetComponentInChildren<AI>().AIName = AIType;
        }

        AI.GetComponentInChildren<AI>().enabled = true;
        AI.SetActive(true);

        return AI;
    }
    public void ReturnAI(string AIType, GameObject AI)
    {
        poolDictionary[AIType].Enqueue(AI);
        AI.SetActive(false);
        AI.GetComponentInChildren<AI>().enabled = false;
    }
}


[System.Serializable] 
public class PoolData
{
    public GameObject AI;
    public int poolSize;
}
