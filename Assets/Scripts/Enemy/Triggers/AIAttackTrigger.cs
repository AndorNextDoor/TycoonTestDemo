using UnityEngine;

public class AIAttackTrigger : MonoBehaviour
{
    private AI _AI;
    [SerializeField] private AiTagTriggers tagName;  

    private void Awake()
    {
        _AI = GetComponentInParent<AI>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag(tagName.ToString()))
        {
            _AI.SetStrikingDistance(true);
            _AI.target = collision.transform;
        }
    }

    private void OnTriggerExit(Collider collision)
    {

        if (collision.transform == _AI.target)
        {
            _AI.SetStrikingDistance(false);
        }
    }

}
