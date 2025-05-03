using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasePlayer : MonoBehaviour
{
    [Header("Player Target")]
    [SerializeField] private Transform player;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float chaseRange;

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Projectile Detection")]
    [SerializeField] private float shieldRange;
    [SerializeField] private LayerMask projectileLayer;
    [SerializeField] private float shieldCooldown; // Cooldown for shield ability

    [Header("Shield Duration")]
    [SerializeField] private float additionalShieldDuration = 0f; // Extra seconds to add to shield duration

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private Vector3 initScale;
    private float cooldownTimer = Mathf.Infinity;
    private float shieldCooldownTimer = Mathf.Infinity;
    private Health playerHealth;
    private bool isShielding = false;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        shieldCooldownTimer += Time.deltaTime;

        if (ProjectileInRange() && shieldCooldownTimer >= shieldCooldown)
        {
            TriggerShield();
        }
        else if (!isShielding && PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage, isShielding);
                }
            }
        }
        else if (!isShielding && Vector2.Distance(enemy.position, player.position) <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    private void TriggerShield()
    {
        if (!isShielding)
        {
            isShielding = true;
            anim.SetBool("moving", false); // Stop moving when shielding
            shieldCooldownTimer = 0; // Reset shield cooldown timer
            anim.SetTrigger("shield");
            StartCoroutine(ShieldDuration());
        }
    }

    private IEnumerator ShieldDuration()
    {
        float animationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength + additionalShieldDuration); // Add extra shield duration
        EndShield();
    }

    private void EndShield()
    {
        isShielding = false;
    }

    private void ChasePlayer()
    {
        anim.SetBool("moving", true);

        // Determine the direction towards the player
        int direction = player.position.x < enemy.position.x ? -1 : 1;

        // Make enemy face the player
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        // Move towards the player
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y, enemy.position.z);
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private bool ProjectileInRange()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * shieldRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * shieldRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, projectileLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * attackRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * shieldRange * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * shieldRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public bool IsCurrentlyShielding()
    {
        return isShielding;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
