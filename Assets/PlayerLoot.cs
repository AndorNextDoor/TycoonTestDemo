using UnityEngine;

public class PlayerLoot : MonoBehaviour
{
    [SerializeField] private int expirienceGain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ProgressionManager.Instance.GetExpirience(expirienceGain);
            Destroy(gameObject);
        }
    }
}
