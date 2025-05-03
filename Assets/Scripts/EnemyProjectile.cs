using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Avoid hitting objects tagged as "enemy"
        if (collision.CompareTag("Enemy") || collision.CompareTag("Key") || collision.CompareTag("Collectible"))
        {
            return;
        }

        // If we hit a non-enemy object, proceed
        hit = true;
        base.OnTriggerEnter2D(collision); // Execute logic from parent script first
        coll.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("explode"); // When the object is a fireball, trigger the explosion animation
        }
        else
        {
            gameObject.SetActive(false); // When this hits any object, deactivate the arrow
        }
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
