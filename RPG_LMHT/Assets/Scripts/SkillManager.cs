using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] GameObject shootSkill;
    [SerializeField] GameObject beam;
    [SerializeField] GameObject finishBeam;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] int shootCooldown = 20;
    [SerializeField] BoxCollider areaTakedamage;
    [SerializeField] LayerMask enemyMask;

    private PlayerController playerController;

    private bool isShooting = false;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (beam.activeSelf)
        {
            Vector3 boxCenter = transform.TransformPoint(areaTakedamage.center);

            Vector3 boxHalfExtents = areaTakedamage.bounds.size;

            Quaternion boxOrientation = transform.rotation;


            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, boxOrientation, enemyMask);

            if (hitColliders.Length > 0)
            {
                StartCoroutine(TakeDamageBySkill());
            }

            if (finishBeam.activeSelf)
            {

                shootSkill.SetActive(false);

                finishBeam.SetActive(false);

                beam.SetActive(false);
            }
        }
    }
    private IEnumerator TakeDamageBySkill()
    {
        Health healthEnemy = playerController.CurrentTarget.GetComponent<Health>();

        healthEnemy.TakeDamage(.01f);

        yield return null;
    }
    public void Shoot()
    {
        GetComponent<NavMeshAgent>().SetDestination(transform.position);

        StartCoroutine(IEShoot());
    }
    private IEnumerator IEShoot()
    {
        textMeshProUGUI.gameObject.SetActive(true);

        isShooting = true;

        shootSkill.SetActive(true);

        icon.color = new Color(1, 1, 1, .35f);

        while (shootCooldown > 0)
        {
            textMeshProUGUI.text = shootCooldown + "";

            shootCooldown--;

            yield return new WaitForSeconds(1f);
        }
        icon.color = Color.white;

        shootCooldown = 20;

        shootSkill.SetActive(false);

        isShooting = false;

        textMeshProUGUI.gameObject.SetActive(false);
    }
}
