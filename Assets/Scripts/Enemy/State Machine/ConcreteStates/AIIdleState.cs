using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(AI AI, AIStateMachine AIStateMachine) : base(AI, AIStateMachine)
    {

    }

    public override void AnimationTrigger(AI.AnimationTriggerType triggerType)
    {
        base.AnimationTrigger(triggerType);

        ally.AIIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        ally.AIIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        ally.AIIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        ally.AIIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ally.AIIdleBaseInstance.DoPhysicsLogic();
    }
}
