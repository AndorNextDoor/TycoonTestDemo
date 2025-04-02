using UnityEngine;

public class AIIdleSOBase : ScriptableObject
{
    protected AI ai;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform mainBuildingTransform;

    public virtual void Initialize(GameObject gameObject, AI AI)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.ai = AI;
    }

    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValuess(); }
    public virtual void DoFrameUpdateLogic() {

        if (ai.IsAggroed)
        {
            ai.StateMachine.ChangeState(ai.ChaseState);
        }

        if (ai.IsWithinStrikingDistance)
        {
            ai.StateMachine.ChangeState(ai.AttackState);
        }
    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType) { }
    public virtual void ResetValuess() { }
}
