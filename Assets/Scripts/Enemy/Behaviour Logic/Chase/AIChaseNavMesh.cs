using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Chase-NavMesh", menuName = "AI Logic/Chase Logic/Chase Nav Mesh")]
public class AIChaseNavMesh: AIChaseSOBase
{
    private NavMeshAgent agent;

    #region State Flow Variables
    private float _exitTimer;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _timeToExitState;
    #endregion

    public override void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        ai.animator.SetBool("WALK", true);
        agent.isStopped = false;
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
        StateCheck();
        Move();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, AI AI)
    {
        base.Initialize(gameObject, AI);
        agent = AI.GetComponent<NavMeshAgent>();
        agent.speed = AI.runSpeed;
    }

    public override void ResetValuess()
    {
        base.ResetValuess();
    }

    private void Move()
    {
        // TO DO: Make that in coroutine
        Vector3 _targetWithOurYCoordinate = new Vector3(ai.target.position.x, ai.transform.position.y, ai.target.position.z);
        agent.SetDestination(_targetWithOurYCoordinate);
    }

    private void StateCheck()
    {
        if (ai.target == null)
        {
            ai.StateMachine.ChangeState(ai.IdleState);
        }

        if (Vector3.Distance(ai.transform.position, ai.target.position) > _maxDistance)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeToExitState)
            {
                ai.StateMachine.ChangeState(ai.IdleState);
            }
        }
        else
        {
            _exitTimer = 0;
        }
    }

    private void FaceCurrentTarget(Transform target)
    {
        Vector3 direction = (target.position - ai.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, 45 * Time.deltaTime);
    }
}
