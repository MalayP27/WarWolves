using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.5f; // Delay before the platform starts to fall
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody2D attached to the platform!");
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static; // Make sure platform is initially static
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TriggerPlatformFall());
        }
    }

    private IEnumerator TriggerPlatformFall()
    {   
        anim.SetTrigger("stepped");
        yield return new WaitForSeconds(fallDelay);
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Make the platform fall by enabling gravity
        }
    }
}
