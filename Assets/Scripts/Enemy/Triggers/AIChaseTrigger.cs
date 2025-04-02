using UnityEngine;

public class AIChaseTrigger : MonoBehaviour
{
    [SerializeField] private AiTagTriggers tagName;
    private AI _AI;

    private void Awake()
    { 
        _AI = GetComponentInParent<AI>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag(tagName.ToString()))
        {
            _AI.SetAggroStatus(true);
            _AI.target = collision.transform;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(tagName.ToString()))
        {
            _AI.SetAggroStatus(false);
        }
    }
}

public enum AiTagTriggers
{
    Enemy,
    Guardian
}
