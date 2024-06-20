using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnWithoutRegionEnemy;


    [SerializeField] float stopDistance = 13f;

    [Header("Components")]
    private Camera mainCamera;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private bool isMoving = false;
    public Transform CurrentTarget { get; set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleTargetFollowing();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == 7) // Layer 7 represents enemy layer
                {
                    SetTarget(hit.transform);
                }
                else
                {
                    ClearTarget();
                }
                MoveToPosition(hit.point);
            }
        }
        CheckIfArrived();
    }

    private void HandleTargetFollowing()
    {
        if (CheckIfInEnemyRegion())
        {
            FollowTarget();
        }
        else
        {
            OnWithoutRegionEnemy?.Invoke(this, EventArgs.Empty);
            ResetFollowing();
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        isMoving = true;
    }

    private void CheckIfArrived()
    {
        if (isMoving && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    isMoving = false;
                }
            }
        }
        animator.SetBool("IsMoving", isMoving);
    }

    private void SetTarget(Transform target)
    {
        CurrentTarget = target;
    }

    private void ClearTarget()
    {
        CurrentTarget = null;
    }

    private void FollowTarget()
    {
        navMeshAgent.stoppingDistance = stopDistance;
        navMeshAgent.updateRotation = false;

        Vector3 direction = (CurrentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void ResetFollowing()
    {
        navMeshAgent.stoppingDistance = 0f;
        navMeshAgent.updateRotation = true;
    }

    public bool CheckIfInEnemyRegion()
    {
        if (CurrentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, CurrentTarget.position);
            return distance <= stopDistance;
        }
        return false;
    }
}
