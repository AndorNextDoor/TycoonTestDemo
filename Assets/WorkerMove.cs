using UnityEngine;
using System.Collections;

public class WorkerMove : MonoBehaviour
{
    [SerializeField] private BuildingUpgradable building;
    [SerializeField] private Transform idlePos;
    [SerializeField] private Transform workPos;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 3f;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        building.OnWorkingStart += MoveTowardsWork;
        building.OnMiningStop += MoveTowardsIdle;
    }

    public void MoveTowardsWork()
    {
        animator.SetBool("WORK", false);
        animator.SetBool("IDLE", false);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToPosition(workPos, "WORK"));
    }

    public void MoveTowardsIdle()
    {
        animator.SetBool("WORK", false);
        animator.SetBool("IDLE", false);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToPosition(idlePos, "IDLE"));
    }

    private IEnumerator MoveToPosition(Transform target, string state)
    {
        animator.SetBool("WALK", true);

        Vector3 targetWithOurYPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        while (Vector3.Distance(transform.position, targetWithOurYPos) > 0.4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWithOurYPos, moveSpeed * Time.deltaTime);
            
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0; // Prevent rotation on X and Z axes
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
            }

            yield return null;
        }

        animator.SetBool("WALK", false);
        animator.SetBool(state, true);
    }
}
