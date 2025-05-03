using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For accessing UI elements

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private float specialAttackCD;
    [SerializeField] private float basicAttackCD;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] tornados;

    [Header("Attack Parameters")]
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask enemyLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Health enemyHealth;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cDTimer = Mathf.Infinity;
    private float basicAttackCDTimer = Mathf.Infinity;

    [Header("Attack Timing")]
    [SerializeField] private float basicAttackDelay = 0.5f;

    [Header("UI Elements")]
    [SerializeField] private Image specialAttackIcon; // UI Image for Special Attack Cooldown
    [SerializeField] private Image basicAttackIcon; // UI Image for Basic Attack Cooldown
    [SerializeField] private Text specialAttackCooldownText; // UI Text for Special Attack Cooldown
    [SerializeField] private Text basicAttackCooldownText; // UI Text for Basic Attack Cooldown

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && cDTimer > specialAttackCD && playerMovement.canAttack())
        {
            SpecialAttack();
        }
        else if (Input.GetKey(KeyCode.O) && basicAttackCDTimer > basicAttackCD)
        {
            BasicAttack();
        }

        cDTimer += Time.deltaTime;
        basicAttackCDTimer += Time.deltaTime;

        UpdateCooldownUI();
    }

    private void SpecialAttack()
    {
        anim.SetTrigger("SwordSwing");
        cDTimer = 0;

        tornados[FindTornados()].transform.position = firePoint.position;
        tornados[FindTornados()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void BasicAttack()
    {
        anim.SetTrigger("SwordSwing");
        basicAttackCDTimer = 0;

        StartCoroutine(DelayedDamage());
    }

    private IEnumerator DelayedDamage()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(basicAttackDelay);

        // Check for enemies in the hitbox and deal damage if found
        if (EnemyInHitBox())
        {
            DamagePlayer();
        }
    }

    private int FindTornados()
    {
        for (int i = 0; i < tornados.Length; i++)
        {
            if (!tornados[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private bool EnemyInHitBox()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    private void UpdateCooldownUI()
    {
        // Update special attack cooldown icon fill amount
        if (specialAttackIcon != null)
        {
            specialAttackIcon.fillAmount = Mathf.Clamp01(cDTimer / specialAttackCD);
        }

        // Update basic attack cooldown icon fill amount
        if (basicAttackIcon != null)
        {
            basicAttackIcon.fillAmount = Mathf.Clamp01(basicAttackCDTimer / basicAttackCD);
        }

        // Update special attack cooldown text
        if (specialAttackCooldownText != null)
        {
            float specialCooldownRemaining = Mathf.Max(0, specialAttackCD - cDTimer);
            specialAttackCooldownText.text = specialCooldownRemaining > 0 ? specialCooldownRemaining.ToString("F1") : "";
        }

        // Update basic attack cooldown text
        if (basicAttackCooldownText != null)
        {
            float basicCooldownRemaining = Mathf.Max(0, basicAttackCD - basicAttackCDTimer);
            basicAttackCooldownText.text = basicCooldownRemaining > 0 ? basicCooldownRemaining.ToString("F1") : "";
        }
    }

    public void ReduceSpecialAttackCooldown(float amount)
    {
        // Ensure cooldown does not go below a reasonable minimum (e.g., 1 second)
        specialAttackCD = Mathf.Max(specialAttackCD - amount, 0.5f);
    }

    public void RestoreSpecialAttackCooldown(float amount)
    {
        // Restore the cooldown by adding the reduced amount back
        specialAttackCD += amount;
    }
}
