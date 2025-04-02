using System.Security.Cryptography;
using UnityEngine;

public class AIAttackState : AIState
{
    public AIAttackState(AI AI, AIStateMachine AIStateMachine) : base(AI, AIStateMachine)
    {

    }

    public override void AnimationTrigger(AI.AnimationTriggerType triggerType)
    {
        base.AnimationTrigger(triggerType);
        ally.AIAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        ally.AIAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        ally.AIAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        ally.AIAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        ally.AIAttackBaseInstance.DoPhysicsLogic();
    }
}
