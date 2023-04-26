using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Basic : MonoBehaviour
{
    [SerializeField]
    private Transform _floorPointer;
    private NavMeshAgent _navMeshAgent;
   // [SerializeField]
   // private List<Transform> _path;
    private bool _forwardFlag = true;
    private int _pathIndex = 0;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _navMeshAgent.SetDestination(_path[0].position);
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void Update()
    {
       // FollowPath();
    }

    private void FollowPath()
    {
        //Basic script for following patrol path
        float distance = Vector3.Distance(_navMeshAgent.destination, _floorPointer.position);
        if (distance < 0.2f)
        {

            if (_forwardFlag)
            {

                _pathIndex++;
                _navMeshAgent.SetDestination(_path[_pathIndex].position);
                if (_pathIndex == _path.Count - 1)
                {
                    _forwardFlag = false;
                }
            }
            if (!_forwardFlag)
            {
                _pathIndex--;
                _navMeshAgent.SetDestination(_path[_pathIndex].position);
                if (_pathIndex == 0)
                {
                    _forwardFlag = true;
                }
            }

        }
        Quaternion _rotDirection = Quaternion.LookRotation(_navMeshAgent.destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotDirection, Time.deltaTime);
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
}
