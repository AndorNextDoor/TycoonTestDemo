using UnityEngine;

public interface IAIMoveable 
{
    Rigidbody rb { get; set; }
    Transform target { get; set; }

    void MoveAI(Transform target);
    void UpdateMainTarget();
    void FaceCurrentTarget(Transform target);
}
