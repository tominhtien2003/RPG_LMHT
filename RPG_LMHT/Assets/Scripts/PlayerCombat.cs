using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCombat : MonoBehaviour
{
    public event EventHandler<OnAttackingEnemyEventArgs> OnAttackingEnemy;
    public class OnAttackingEnemyEventArgs : EventArgs
    {
        public Transform currentEnemy { get; set; }
    }

    [SerializeField] private AnimationClip[] animationClipAttacks;

    private int indexCurrent = -1;
    private Animator animator;

    private bool isAttacking = false;
    private PlayerController playerController;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        //HandleAttackInput();
    }

    public void HandleAttackInput()
    {
        if (!isAttacking)
        {
            StopMovement();
            
            StartCoroutine(AttackRoutine());
        }
    }
    private int GetIndexCurrentRoutine()
    {
        return indexCurrent = ((++indexCurrent) % animationClipAttacks.Length);
    }
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        float attackCooldown = animationClipAttacks[GetIndexCurrentRoutine()].length;

        if (indexCurrent == 0)
        {
            animator.SetTrigger("AttackL");
        }
        else
        {
            animator.SetTrigger("AttackR");
        }
        yield return new WaitForSeconds(attackCooldown/2);

        if (playerController.CheckIfInEnemyRegion())
        {
            playerController.CurrentTarget.GetComponent<Health>().TakeDamage(.1f);
            OnAttackingEnemy?.Invoke(this, new OnAttackingEnemyEventArgs { currentEnemy = playerController.CurrentTarget }) ;
        }
        yield return new WaitForSeconds(attackCooldown / 2);

        isAttacking = false;
    }

    private void StopMovement()
    {
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
    //private bool CheckButtonPressed()
    //{

    //}
}
