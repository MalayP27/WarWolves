using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostCollectible : MonoBehaviour
{
    [SerializeField] private float speedBoostMultiplier = 2f; // Multiplier for the speed boost
    [SerializeField] private float boostDuration = 5f; // Duration of the speed boost
    [SerializeField] private Color flashColor = Color.magenta; // Color to flash during the boost

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();

            if (playerMovement != null && playerSprite != null)
            {
                StartCoroutine(ApplySpeedBoost(playerMovement, playerSprite));
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

    private IEnumerator ApplySpeedBoost(PlayerMovement playerMovement, SpriteRenderer playerSprite)
    {
        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedBoostMultiplier;

        Color originalColor = playerSprite.color;
        float flashInterval = 0.1f; // Interval for flashing color
        float elapsedTime = 0f;

        while (elapsedTime < boostDuration)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval * 2;
        }

        // Reset player speed and color after the boost duration
        playerMovement.speed = originalSpeed;
        playerSprite.color = originalColor;

        // After the coroutine ends, destroy the game object if it is still inactive
        if (!gameObject.activeSelf)
        {
            Destroy(gameObject);
        }
    }
}
