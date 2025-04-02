using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Idle-Searching-The-Dungeon", menuName = "AI Logic/Idle Logic/Searching the dungeon")]
public class AIIdleSearchingTheDungeon : AIIdleSOBase
{
    private NavMeshAgent agent;
    private Transform currentPointOfInterest;

    public override void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        ai.animator.SetBool("WALK", true);
        agent.isStopped = false;

        agent.speed = ai.moveSpeed;
        currentPointOfInterest = AIPathingManager.Instance.GetSquadPointOfInterest();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        ai.animator.SetBool("WALK", false);
        agent.isStopped = true;
        agent.SetDestination(ai.transform.position);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        MoveAI();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, AI AI)
    {
        base.Initialize(gameObject, AI);

        AIPathingManager.Instance.OnPointReached += AIOnPointReached;
        AIPathingManager.Instance.OnLastPointReached += AIOnLastPointReached;

        agent = AI.GetComponent<NavMeshAgent>();
        agent.speed = AI.moveSpeed;
    }

    private void AIOnLastPointReached()
    {
        currentPointOfInterest = AIPathingManager.Instance.GetReturnPoint();
    }

    private void AIOnPointReached()
    {
        currentPointOfInterest = AIPathingManager.Instance.GetSquadPointOfInterest();
    }

    public void MoveAI()
    {
        Vector3 _targetWithOurYCoordinate = new Vector3(currentPointOfInterest.position.x, ai.transform.position.y, currentPointOfInterest.position.z);
        agent.SetDestination(_targetWithOurYCoordinate);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (agent.hasPath || agent.velocity.magnitude == 0f)
            {
                AIPathingManager.Instance.OnAIPointReached();
            }
        }
    }

    public void FaceCurrentTarget()
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
