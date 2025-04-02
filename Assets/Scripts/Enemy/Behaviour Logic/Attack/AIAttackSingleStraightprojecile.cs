using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Single-Straight-Projectile", menuName = "AI Logic/Attack Logic/Single Straight Projectile")]
public class AIAttackSingleStraightprojecile : AIAttackSOBase
{
    [SerializeField] private float _timer;
    [SerializeField] private float _timeBetweenShots = 2f;

    [SerializeField] private float _exitTimer;
    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 10f;

    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private GameObject bulletPrefab;

    public override void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        Attack();
        StateCheck();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, AI AI)
    {
        base.Initialize(gameObject, AI);
    }

    public override void ResetValuess()
    {
        base.ResetValuess();
    }
    private void Attack()
    {
        if (_timer > _timeBetweenShots)
        {
            _timer = 0f;

            Vector3 dir = (ai.target.position - ai.transform.position).normalized;

            GameObject bullet = GameObject.Instantiate(bulletPrefab, ai.transform.position, Quaternion.identity);
            Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.angularVelocity = dir * bulletSpeed;
        }
    }

    private void StateCheck()
    {

        if (Vector3.Distance(ai.transform.position, ai.target.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                ai.StateMachine.ChangeState(ai.ChaseState);
            }
        }
        else
        {
            _exitTimer = 0;
        }
        _timer += Time.deltaTime;
    }
}
