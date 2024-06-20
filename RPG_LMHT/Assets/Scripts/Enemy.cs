using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AnimationClip animationClipAttack;

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
        player.GetComponent<PlayerCombat>().OnAttackingEnemy += PlayerCombat_OnAttackingEnemy;
        player.GetComponent<PlayerController>().OnWithoutRegionEnemy += Enemy_OnWithoutRegionEnemy;
    }

    private void Enemy_OnWithoutRegionEnemy(object sender, System.EventArgs e)
    {
        attackMode = false;
    }

    private void PlayerCombat_OnAttackingEnemy(object sender, System.EventArgs e)
    {
        attackMode = true;
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
    private IEnumerator IEAttack()
    {
        isAttacking = true;

        float attackCooldown = animationClipAttack.length;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown / 2);

        player.GetComponent<Health>().TakeDamage(.05f);

        yield return new WaitForSeconds(attackCooldown / 2 + 1f);

        isAttacking = false;
    }
}
