using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeMoveCollectible : MonoBehaviour
{
    [SerializeField] private float freezeDuration = 3f; // Duration of the freeze
    [SerializeField] private GameObject freezeBG; // Object to enable during freeze duration

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FreezeEnemies());
        }
    }

    private IEnumerator FreezeEnemies()
    {
        // Enable the effect object
        if (freezeBG != null)
        {
            freezeBG.SetActive(true);
        }

        // Find all GameObjects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemyPatrols = GameObject.FindGameObjectsWithTag("EnemyPatrol");

        // List to store the enemy scripts that control movement
        List<MonoBehaviour> enemyMovementScripts = new List<MonoBehaviour>();

        // Loop through each enemy patrol to disable their movement
        foreach (GameObject patrol in enemyPatrols)
        {
            EnemyPatrol EPScript = patrol.GetComponent<EnemyPatrol>();

            if (EPScript != null)
            {
                EPScript.enabled = false; // Disable the EnemyPatrol script to stop enemy movement
                enemyMovementScripts.Add(EPScript); // Store reference for enabling later
            }
        }

        // Loop through each enemy to disable their movement
        foreach (GameObject enemy in enemies)
        {
            MeleeEnemy meleeScript = enemy.GetComponent<MeleeEnemy>();
            RangedEnemy rangedScript = enemy.GetComponent<RangedEnemy>();

            if (meleeScript != null)
            {
                meleeScript.enabled = false; // Disable the MeleeEnemy script to stop enemy movement
                enemyMovementScripts.Add(meleeScript); // Store reference for enabling later
            }
            else if (rangedScript != null)
            {
                rangedScript.enabled = false; // Disable the RangedEnemy script to stop enemy movement
                enemyMovementScripts.Add(rangedScript); // Store reference for enabling later
            }
        }

        // Wait for the freeze duration
        yield return new WaitForSeconds(freezeDuration);

        // Re-enable movement
        foreach (MonoBehaviour movementScript in enemyMovementScripts)
        {
            if (movementScript != null)
            {
                movementScript.enabled = true; // Enable movement scripts again
            }
        }

        // Disable the effect object
        if (freezeBG != null)
        {
            freezeBG.SetActive(false);
        }

        // Destroy the collectible after the effect ends
        Destroy(gameObject);
    }
}
