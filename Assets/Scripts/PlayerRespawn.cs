using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For accessing UI elements

public class PlayerRespawn : MonoBehaviour
{
    private Transform currentCheckpoint;
    private Health playerHealth;
    [SerializeField] private Text deathCounterText; // UI Text element to display the death counter
    private int deathCount = 0; // Counter to keep track of player deaths

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        UpdateDeathCounterUI(); // Initialize UI with the initial death count
    }

    public void Respawn()
    {
        deathCount++; // Increment death count every time player respawns
        UpdateDeathCounterUI(); // Update the UI text to reflect the new death count
        
        playerHealth.Respawn(); // Restore player health and reset animation
        transform.position = currentCheckpoint.position; // Move player to checkpoint location

        // Move the camera directly to the checkpoint's position
        if (Camera.main != null && currentCheckpoint != null)
        {
            Vector3 newCameraPosition = new Vector3(currentCheckpoint.position.x, currentCheckpoint.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = newCameraPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }

    private void UpdateDeathCounterUI()
    {
        if (deathCounterText != null)
        {
            deathCounterText.text = deathCount.ToString();
        }
    }
}
