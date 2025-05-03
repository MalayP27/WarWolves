using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackCooldownCollectible : MonoBehaviour
{
    [SerializeField] private float cooldownReductionAmount = 2f; // Amount to reduce the special attack cooldown
    [SerializeField] private float boostDuration = 5f; // Duration of the cooldown reduction effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttacks playerAttacks = collision.GetComponent<PlayerAttacks>();

            if (playerAttacks != null)
            {
                StartCoroutine(ApplyCooldownReduction(playerAttacks));
                HideCollectible(); // Hide the collectible once it's picked up
            }
        }
    }

    private void HideCollectible()
    {
        // Hide the collectible by disabling the sprite renderer and collider
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private IEnumerator ApplyCooldownReduction(PlayerAttacks playerAttacks)
    {
        // Reduce the special attack cooldown
        playerAttacks.ReduceSpecialAttackCooldown(cooldownReductionAmount);

        // Wait for the duration of the cooldown reduction effect
        yield return new WaitForSeconds(boostDuration);

        // Restore the original cooldown
        playerAttacks.RestoreSpecialAttackCooldown(cooldownReductionAmount);
    }
}
