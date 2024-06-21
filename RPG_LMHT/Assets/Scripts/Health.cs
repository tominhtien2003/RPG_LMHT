using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject gfx;
    private Health healthPlayer;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        healthPlayer = GameObject.Find("Player").GetComponent<Health>();
    }
    private void LateUpdate()
    {
        healthBar.transform.forward = Camera.main.transform.forward;
    }
    public void TakeDamage(float damage)
    {
        if (healthBar.value >= damage)
        {
            healthBar.value -= damage;
        }
        else
        {
            healthBar.value = 0f;           
            StartCoroutine(IEDestroyEnenmy());
        } 
    }
    private IEnumerator IEDestroyEnenmy()
    {
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1f);

        if (gameObject.layer == 7)
        {
            healthPlayer.transform.GetComponent<PlayerController>().CurrentTarget = null;

            healthPlayer.healthBar.value = 1f;
        }
        gfx.SetActive(false);

        healthBar.gameObject.SetActive(false);

        StartCoroutine(IERevival());
    }
    private IEnumerator IERevival()
    {
        yield return new WaitForSeconds(10f);

        gfx.SetActive(true);

        healthBar.gameObject.SetActive(true);
        healthBar.value = 1f;
    }
}
