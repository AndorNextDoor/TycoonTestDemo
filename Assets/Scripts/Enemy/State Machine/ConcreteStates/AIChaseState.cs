using UnityEngine;

public class AIChaseState : AIState
{
    public AIChaseState(AI AI, AIStateMachine AIStateMachine) : base(AI, AIStateMachine)
    {

    }

    public override void AnimationTrigger(AI.AnimationTriggerType triggerType)
    {
        base.AnimationTrigger(triggerType);
        ally.AIChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        ally.AIChaseBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        ally.AIChaseBaseInstance.DoExitLogic();
    }
        
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        ally.AIChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ally.AIChaseBaseInstance.DoPhysicsLogic();
    }
}
