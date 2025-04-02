using System;
using UnityEngine;

public class AI : MonoBehaviour, IDamagable, ITriggerCheckable
{
    [field: SerializeField] public string AIName { get; set; }
    [field: SerializeField] public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    [field: SerializeField] public int damage { get; set; }

    [field: SerializeField] public float moveSpeed;

    [field: SerializeField] public float runSpeed;

    [field: SerializeField] public float turnSpeed;

    public Transform target { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
    public Animator animator { get; set; }

    private HealthBar healthBar;

    [SerializeField] private GameObject droppedLoot;

    public event Action OnDeath;
    public event EventHandler<GameObject> OnDeathGameObject;

    #region State Machine Variables

    public AIStateMachine StateMachine { get; set; }
    public AIIdleState IdleState { get; set; }
    public AIChaseState ChaseState { get; set; }
    public AIAttackState AttackState { get; set; }
    #endregion

    #region ScriptableObject State Machine Variables
    [SerializeField] private AIIdleSOBase AIIdleBase;
    [SerializeField] private AIChaseSOBase AIChaseBase;
    [SerializeField] private AIAttackSOBase AIAttackBase;
    public AIIdleSOBase AIIdleBaseInstance { get; set; }
    public AIChaseSOBase AIChaseBaseInstance { get; set; }
    public AIAttackSOBase AIAttackBaseInstance { get; set; }
    #endregion

    #region Idle State

    #endregion

    //Procedures
    #region Standart Procedures
    private void OnEnable()
    {
        ResetAI();
    }
    private void Awake()
    {
        AIIdleBaseInstance = Instantiate(AIIdleBase);
        AIChaseBaseInstance = Instantiate(AIChaseBase);
        AIAttackBaseInstance = Instantiate(AIAttackBase);

        StateMachine = new AIStateMachine();

        IdleState = new AIIdleState(this, StateMachine);
        ChaseState = new AIChaseState(this, StateMachine);
        AttackState = new AIAttackState(this, StateMachine);

        animator = GetComponentInChildren<Animator>();

        healthBar = GetComponent<HealthBar>();
        healthBar.InitializeHealthBar(maxHealth);
    }

    private void Start()
    {
        AIIdleBaseInstance.Initialize(gameObject, this);
        AIChaseBaseInstance.Initialize(gameObject, this);
        AIAttackBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        if (!this.enabled) return;
        StateMachine.CurrentAIState.FrameUpdate();

        if(IsAggroed || IsWithinStrikingDistance)
        {
            if (!target.gameObject.activeSelf || target == null)
            {
                SetAggroStatus(false);
                SetStrikingDistance(false);

                target = null;
                StateMachine.ChangeState(IdleState);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
         if (!this.enabled) return;
        StateMachine.CurrentAIState.PhysicsUpdate();
    }
    #endregion

    #region Health/Die Functions
    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.ChangeHealthValue(currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    public void Die()
    {
        //Play Animation

        if(droppedLoot != null)
        {
            Instantiate(droppedLoot, transform.position, Quaternion.identity);
        }
        SetAggroStatus(false);
        StateMachine.ChangeState(IdleState);
        transform.position = new Vector3(-999, -999, -999);
        AIPool.Instance.ReturnAI(AIName, gameObject);

        ResetAI();
        OnDeath?.Invoke();
        OnDeathGameObject?.Invoke(this, gameObject);
    }
    #endregion

    public void ResetAI()
    {
        currentHealth = maxHealth;
        if(StateMachine.CurrentAIState != IdleState && StateMachine.CurrentAIState != null)
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    #region Target Functions


    #endregion

    #region Animation Triggers

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentAIState.AnimationTrigger(triggerType);
    }

    #region Distance Checks

    public void SetAggroStatus(bool aggroStatus)
    {
        IsAggroed = aggroStatus;
    }

    public void SetStrikingDistance(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    #endregion
    public enum AnimationTriggerType
    {
        Attack,
        PlayFootstepsSound
    }

    #endregion
}
