using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EverySecondEvent : UnityEvent<Enemy>
{

}
public abstract class Enemy : MonoBehaviour
{
    public abstract string Name { get; }



    [SerializeField] protected AudioClip[] movmentSound;
    [SerializeField] protected AudioClip[] attackSound;
    [SerializeField] protected AudioClip[] deathSound;
    [SerializeField] protected AudioClip[] takeDamageSound;
    [SerializeField] protected Transform attackPoint;

    protected int _health;
    protected float _speed;
    protected float _range;
    protected float _attackCooldown;
    protected float _attackValue;
    protected float _attackRadius;

    protected Animator _animator;

    public EverySecondEvent EverySecond;

    public delegate void DeathDisplayEventHandler(Transform displayLocation, bool adds);
    public event DeathDisplayEventHandler DeathDisplayEvent;

    [Header("AI")]
    public NavMeshAgent navMeshAgent;
    public Transform _floorPointer;
    public List<Transform> _path;
    public State movement;
    public State attack;
    public bool HasDeathAnimation = true;



    [Header("Reward Location")]
    public Transform deathDisplayTransform;

    protected State currentState;
    //states
    protected bool IsDead;
    private bool KnockedBack = false;
    protected AudioSource _audioSource;

    public static readonly int _walk = Animator.StringToHash("Walk");
    public static readonly int AAttack = Animator.StringToHash("Attack");
    public static readonly int _die = Animator.StringToHash("Death");

    private void Awake()
    {
        /*
        var loadData = Resources.Load<EnemyData>($"Data/Enemies/ENEMY_{Name}");

        _health = loadData.Health;
        _speed = loadData.Speed;
        _range = loadData.Range;
        _attackCooldown = loadData.AttackCooldown;
        _attackValue = loadData.AttackValue;
        _attackRadius = loadData.AttackRadius;
        */

    }

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        SetNearestTarget();
        ChangeState(movement);
    }


    private void Update()
    {

    }

    private void LateUpdate()
    {
        if (IsDead) return;
        currentState.UpdateState(this);
    }

    private void StopAudioSource()
    {

       // SoundManager.Instance.StopOnGivenAudioSource(_audioSource);
    }

    public virtual void Attack()
    {
     //   SoundManager.Instance.PlayOneShotOnGivenAudioSource(_audioSource, attackSound);
    }
    public virtual void Move()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            _animator.Play(_walk);



       // if (!GameManager.Instance.IsGamePaused)
            //SoundManager.Instance.PlayOnGivenAudioSource(_audioSource, movmentSound);
    }

    public virtual void TakeHP(int hp, float knockback = 0, Transform attacker = null)
    {


        //SoundManager.Instance.PlayOneShotOnGivenAudioSource(_audioSource, takeDamageSound);
        if (_health <= 0)
        {
            Die();
        }
    }


    public virtual void Die()
    {

        IsDead = true;

        if (HasDeathAnimation)
            _animator.Play(_die);
        Debug.Log("Enemy just died ...");

        navMeshAgent.isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;
        //SoundManager.Instance.PlayOneShotOnGivenAudioSource(_audioSource, deathSound);


    }

    public virtual void DeathEvent()
    {
        Invoke(nameof(DestroyEnemy), 1f);
    }

    protected virtual void SetNearestTarget()
    {
        
    }

    public void ChangeState(State newState)
    {


        if (currentState != null) currentState.ExitState(this);

        if (newState != null)
        {
            currentState = newState;
            currentState.StartState(this);
        }

    }

    private void DestroyEnemy()
    {

        DeathDisplayEvent(deathDisplayTransform, true);
        DeathDisplayEvent = null;
        Destroy(this.gameObject);
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public float GetRange()
    {
        return _range;
    }

    public float GetAttackCooldown()
    {
        return _attackCooldown;
    }
    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
    public Animator GetAnimator()
    {
        return _animator;
    }





}
