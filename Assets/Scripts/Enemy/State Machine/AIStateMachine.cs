using UnityEngine;

public class AIStateMachine 
{
    public AIState CurrentAIState { get; set; }

    public void Initialize(AIState startingState)
    {
        CurrentAIState = startingState;
        CurrentAIState.EnterState();
    }

    public void ChangeState(AIState newState)
    {
        CurrentAIState.ExitState();
        CurrentAIState = newState;
        CurrentAIState.EnterState();
    }
}
