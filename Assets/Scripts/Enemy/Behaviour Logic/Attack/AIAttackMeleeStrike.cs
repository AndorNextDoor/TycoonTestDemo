using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Melee-Strike", menuName = "AI Logic/Attack Logic/Melee Strike")]
public class AIAttackMeleeStrike : AIAttackSOBase
{
    #region State Flow Variables
    private float _exitTimer;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _timeToExitState;
    #endregion

    #region Attack Variables
    [SerializeField] private string ATTACK_POINT_TAG;
    [SerializeField] private AiTagTriggers tagName;

    private float _attackTimer;
    [SerializeField] private float _attackDelay;
    private bool IsAttacking;

    private Transform attackPoint;
    [SerializeField] private float attackRange;

    [SerializeField] private int rotateSpeed = 30;
    #endregion

    public override void DoAnimationTriggerEventLogic(AI.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

        if(triggerType == AI.AnimationTriggerType.Attack)
        {
            DealDamage();
        }
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

        DoAttackCheck();
        StateCheck();
        FaceCurrentTarget(ai.target);
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Initialize(GameObject gameObject, AI AI)
    {
        base.Initialize(gameObject, AI);

        foreach (Transform child in AI.transform)
        {
            if (child.CompareTag(ATTACK_POINT_TAG))
            {
                attackPoint = child.transform;
                break;
            }

        }
    }

    public override void ResetValuess()
    {
        base.ResetValuess();
    }
    private void DoAttackCheck()
    {
        if (IsAttacking) return;

        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackDelay)
        {
            _attackTimer = -999;
            ai.animator.SetTrigger("ATTACK");
        }
    }

    private void DealDamage()
    {
        Collider[] collisions = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider collider in collisions)
        {
            if (!collider.CompareTag(tagName.ToString()))
                continue;

            if (collider.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.Damage(ai.damage);
            }
        }
        IsAttacking = false;
        _attackTimer = 0;
    }

    private void StateCheck()
    {
        if (ai.target == null)
        {
            ai.SetAggroStatus(false);
            ai.SetStrikingDistance(false);
            ai.StateMachine.ChangeState(ai.IdleState);
        }

        if (Vector3.Distance(ai.transform.position, ai.target.position) > _maxDistance)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeToExitState)
            {
                ai.StateMachine.ChangeState(ai.ChaseState);
            }
        }
        else
        {
            _exitTimer = 0;
        }
    }

    private void FaceCurrentTarget(Transform target)
    {
        Vector3 direction = (target.position - ai.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }
}
