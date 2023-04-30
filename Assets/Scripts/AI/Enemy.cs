using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class EverySecondEvent : UnityEvent<Enemy>
{

}
public class Enemy : MonoBehaviour
{
    public string Name { get; }
    public bool InstaKills = false;

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


    [Header("AI")]
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    public Transform _floorPointer;
    public GameObject PathHeader;
    [HideInInspector]
    public int _pathIndex;
    [HideInInspector]
    public bool _forwardFlag;

    //[HideInInspector]
    public List<Transform> _path;
    //states
    public State patrol;
    public State chase;
    public State attack;
    public State stun;
    public bool HasDeathAnimation = true;
    [HideInInspector]
    public bool canSeePlayer = false;

    /*
    [Header("FOV")]
    public Light spotLight;
    public float radius;
    [Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    */

    [HideInInspector]
    public GameObject playerRef;

    [HideInInspector]
    public float stunTime = 3f;

    [Header("Chase")]
    [HideInInspector]
    public Vector3 lastSeenPos;

    [Header("Reward Location")]
    public Transform deathDisplayTransform;

    [SerializeField]
    protected State currentState;
    protected bool IsDead = false;
    protected AudioSource _audioSource;
    [HideInInspector]
    public MMConeOfVision coneOfVision;
    [HideInInspector]
    public State previousState;


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
        if(PathHeader != null)
        {
            CreatePath();
            coneOfVision = GetComponent<MMConeOfVision>();
            currentState = patrol;
        }
        

    }


    private void Start()
    {
        _animator = this.GetComponent<Animator>();
       // _audioSource = GetComponent<AudioSource>();
        _pathIndex = 0;
        _forwardFlag = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerRef = GameManager.Instance.GetPlayer();
        ChangeState(patrol);
        //StartCoroutine(FOVRoutine());

    }

    /* Old way of finding player
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    */

    private void CreatePath()
    {
        Transform[] pathPointers = PathHeader.GetComponentsInChildren<Transform>();
        _path = new List<Transform>();
        foreach(Transform tr in pathPointers)
        {
            if(tr != PathHeader.transform)
            {
                _path.Add(tr);
            }
        }
    }

    private void LateUpdate()
    {
        if (IsDead) return;

        if (GameManager.Instance.isGameFrozen || GameManager.Instance.isGamePaused)
        {
            navMeshAgent.SetDestination(transform.position);
            return;
        }
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


    private void OnCollisionEnter(Collision collision)
    {
        if(!InstaKills) { return; }
        if (collision.gameObject.TryGetComponent<IsometricCharacterController>(out IsometricCharacterController player))
        {
            player.KillPlayer();
        }
    }


}
