using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class Enemy : MonoBehaviour,IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField,Range(0f,20f)] float chasingArea = 10f;
    [SerializeField,Range(0f,10f)] float attackRadius = 4f;
    [SerializeField] float damagePerShot = 10f;
    [SerializeField] float secondsBetweenShots = 0.1f;
    [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;
    float currentHealthPoints;
    AICharacterControl aiCharacterControl = null;
    NavMeshAgent nmAgent = null;
    GameObject player = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
        nmAgent = GetComponent<NavMeshAgent>();
        nmAgent.stoppingDistance = attackRadius;
        currentHealthPoints = maxHealthPoints;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= chasingArea)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }

        if(distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);
        }
        if(distanceToPlayer > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if(currentHealthPoints <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSocket.transform.position, Quaternion.identity);
        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        projectileComponent.SetShooter(gameObject);

        var direction = ((player.transform.position+aimOffset) - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.GetDefaultProjectileSpeed();

        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, chasingArea);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
