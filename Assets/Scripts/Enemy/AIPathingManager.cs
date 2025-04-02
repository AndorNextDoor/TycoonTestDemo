using System;
using System.Collections;
using UnityEngine;

public class AIPathingManager : MonoBehaviour
{
    public static AIPathingManager Instance;

    [SerializeField] private Transform[] walkingToPositions;
    [SerializeField] private Transform returnPoint;

    public event Action OnLastPointReached;
    public event Action OnPointReached;
    private int currentSquadPoint = -1;

    private bool reachOnCooldown;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ReshufflePositions();
    }

    private void ReshufflePositions()
    {
        for (int i = 0; i < walkingToPositions.Length; i++)
        {
            Transform tmp = walkingToPositions[i];
            int r = UnityEngine.Random.Range(i, walkingToPositions.Length);
            walkingToPositions[i] = walkingToPositions[r];
            walkingToPositions[i] = tmp;
        }
    }

    public void OnAIPointReached()
    {
        if (reachOnCooldown)
            return;

        StartCoroutine(OnPointReachedCooldown());

        currentSquadPoint++; 
        if(currentSquadPoint >= walkingToPositions.Length)
        {
            currentSquadPoint = -1;
            OnLastPointReached?.Invoke();
        }
        else
        {
            OnPointReached?.Invoke();   
        }
    }

    IEnumerator OnPointReachedCooldown()
    {
        reachOnCooldown = true; 
        yield return new WaitForSeconds(3);
        reachOnCooldown = false;
    }

    public Transform GetSquadPointOfInterest()
    {
        if(currentSquadPoint == -1)
        {
            return walkingToPositions[currentSquadPoint + 1];
        }
        return walkingToPositions[currentSquadPoint];
    }

    public Transform GetReturnPoint()
    {
        return returnPoint;
    }
}
