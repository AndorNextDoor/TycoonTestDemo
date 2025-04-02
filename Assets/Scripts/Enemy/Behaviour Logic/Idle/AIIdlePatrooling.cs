using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Idle-AI-Idle-Patrooling", menuName = "AI Logic/Idle Logic/AI-Idle-Patrooling")]
public class AIIdlePatrooling : AIIdleSOBase
{
    private NavMeshAgent agent;

    public float patrolRadius = 5f; 
    public float waitTime = 2f; 
    private Vector3 startPosition;
    private float timer;

    public override void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        agent.isStopped = false;

        agent.speed = ai.moveSpeed;
        timer = waitTime;

        // Do logic for patrooling
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        ai.animator.SetBool("WALK", false);
        agent.isStopped = true;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        FaceCurrentTarget();
        PatroolArount();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, AI AI)
    {
        base.Initialize(gameObject, AI);

        startPosition = transform.position;

        agent = AI.GetComponent<NavMeshAgent>();
        agent.speed = AI.moveSpeed;
    }

    private void PatroolArount()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Vector3 randomPoint = GetRandomPatrolPoint();
                agent.SetDestination(randomPoint);
                timer = waitTime;
            }
            else
            {
                ai.animator.SetBool("WALK", false);
            }
        }

    }

    Vector3 GetRandomPatrolPoint()
    {
        for (int i = 0; i < 10; i++) 
        {
            Vector3 randomPos = startPosition + (Random.insideUnitSphere * patrolRadius);
            randomPos.y = ai.transform.position.y; // Keep the same height

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 1.0f, NavMesh.AllAreas))
            {
                ai.animator.SetBool("WALK", true);
                return hit.position; 
            }
        }
        ai.animator.SetBool("WALK", true);
        return startPosition; // Fallback to starting position
    }

    private void FaceCurrentTarget()
    {
        int rotateSpeed = 60;

        Vector3 direction = (agent.destination - ai.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    public override void ResetValuess()
    {
        base.ResetValuess();
    }
}
