using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationClipAttacks;
    [SerializeField] private string[] nameTriggerAnimator;

    private int indexAnimationClipAttack = -1;
    private Transform player;
    private Animator animator;
    private bool attackMode = false;
    private bool isAttacking = false;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        player.GetComponent<PlayerCombat>().OnAttackingEnemy += Enemy_OnAttackingEnemy;
        player.GetComponent<PlayerController>().OnWithoutRegionEnemy += Enemy_OnWithoutRegionEnemy;
    }

    private void Enemy_OnAttackingEnemy(object sender, PlayerCombat.OnAttackingEnemyEventArgs e)
    {
        if (e.currentEnemy == transform)
        {
            attackMode = true;
        }
    }

    private void Enemy_OnWithoutRegionEnemy(object sender, System.EventArgs e)
    {
        attackMode = false;
    }
    private void Update()
    {
        if (attackMode)
        {
            HandleAttack();
        }
    }
    private void HandleAttack()
    {
        if (!isAttacking && player.GetComponent<PlayerController>().CheckIfInEnemyRegion())
        {
            StartCoroutine(IEAttack());
        }
    }
    private int GetIndexOfAnimationClipAttacks()
    {
        return indexAnimationClipAttack = (++indexAnimationClipAttack) % animationClipAttacks.Length;
    }
    private IEnumerator IEAttack()
    {
        isAttacking = true;

        int idx = GetIndexOfAnimationClipAttacks();

        float attackCooldown = animationClipAttacks[idx].length;

        animator.SetTrigger(nameTriggerAnimator[idx]+"");

        yield return new WaitForSeconds(attackCooldown / 2);

        player.GetComponent<Health>().TakeDamage(.05f);

        yield return new WaitForSeconds(attackCooldown / 2 + 1f);

        isAttacking = false;
    }
}
