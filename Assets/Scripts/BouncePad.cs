using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f; // Force to apply when player jumps on the pad
    [SerializeField] private float maxVelocity = 15f; // Maximum allowed bounce speed for the player

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Limit the player's upward velocity
                if (playerRb.velocity.y < maxVelocity)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y + bounceForce, 0, maxVelocity));
                }
            }
        }
    }
}
