using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlowdownBlock : MonoBehaviour
{
    [SerializeField] private float movementReductionFactor = 0.5f; // Factor by which the movement speed is reduced

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                ApplySlowdown(playerMovement);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                RemoveSlowdown(playerMovement);
            }
        }
    }

    private void ApplySlowdown(PlayerMovement playerMovement)
    {
        playerMovement.speed *= movementReductionFactor;
        playerMovement.canJump = false; // Disable jumping entirely when on the block
    }

    private void RemoveSlowdown(PlayerMovement playerMovement)
    {
        playerMovement.speed /= movementReductionFactor;
        playerMovement.canJump = true; // Re-enable jumping when exiting the block
    }
}