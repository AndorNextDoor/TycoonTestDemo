using UnityEngine;

public class AIState
{
    protected AI ally;
    protected AIStateMachine AIStateMachine;

    public AIState(AI AI, AIStateMachine AIStateMachine)
    {
        this.ally = AI;
        this.AIStateMachine = AIStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTrigger(AI.AnimationTriggerType triggerType) { }
    public virtual void OnCollisionEnter(Collider collider) { }
}
