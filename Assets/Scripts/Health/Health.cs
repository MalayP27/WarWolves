using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Animations;

public class Health : MonoBehaviour
{
    [SerializeField] public float startingHP;
    public float currentHP {get; private set;}
    private Animator anim;
    private Boolean dead;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private void Awake(){
        currentHP = startingHP;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage){
        if (invulnerable) return;
        currentHP = Mathf.Clamp(currentHP - damage, 0, startingHP);

        if (currentHP > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
            }
        }

    }

    public void TakeDamage(float damage, bool isShielding)
    {
        if (invulnerable || isShielding) return;
        currentHP = Mathf.Clamp(currentHP - damage, 0, startingHP);

        if (currentHP > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");

                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                dead = true;
            }
        }
    }



    public void AddHealth(float _value)
    {
        currentHP = Mathf.Clamp(currentHP + _value, 0, startingHP);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHP);
        anim.ResetTrigger("Die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());

        //Activate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = true;
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
