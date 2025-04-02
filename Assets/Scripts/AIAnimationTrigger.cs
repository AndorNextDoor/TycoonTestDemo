using UnityEngine;

public class AIAnimationTrigger : MonoBehaviour
{
    private AI ai;

    private void Awake()
    {
        ai = transform.parent.GetComponent<AI>();
    }

    public void TriggerAnimationTrigger(AI.AnimationTriggerType animationTriggerType)
    {
        ai.AnimationTriggerEvent(animationTriggerType);
    }
}
