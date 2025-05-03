using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class KeyChestInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the Player GameObject
    [SerializeField] public GameObject interactionPromptText; // Reference to the InteractionPromptText GameObject
    [SerializeField] public GameObject treasure; // Reference to the key object to be enabled after puzzle completion
    public Animator anim;

    private bool isPlayerInRange = false;
    private List<MonoBehaviour> scriptsToDisable = new List<MonoBehaviour>();

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Disable all scripts regarding objects with the tag 'Enemy' and 'Player'
            DisableScriptsWithTag("Enemy");
            DisableScriptsWithTag("EnemyPatrol");
            DisableScriptsWithTag("Player");

            // Load the KeyChestPuzzle scene
            SceneManager.LoadScene("KeyChestPuzzle", LoadSceneMode.Additive);

            Debug.Log("Player interacted with the chest!");
        }

        // Ensure the interaction prompt text (or canvas) does not flip with the player's movement
        if (interactionPromptText != null)
        {
            Vector3 promptScale = interactionPromptText.transform.localScale;
            promptScale.x = player.transform.localScale.x > 0 ? Mathf.Abs(promptScale.x) : -Mathf.Abs(promptScale.x);
            interactionPromptText.transform.localScale = promptScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPromptText.SetActive(true); // Show the interaction prompt
            Debug.Log("Player is in range of the chest. Press 'E' to interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPromptText.SetActive(false); // Hide the interaction prompt
            Debug.Log("Player left the range of the chest.");
        }
    }

    private void DisableScriptsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
                scriptsToDisable.Add(script);
            }
        }
    }

    public void EnableDisabledScripts()
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = true;
        }
        scriptsToDisable.Clear();
    }

    public void EnableKeyObject()
    {
        if (treasure != null)
        {
            treasure.SetActive(true);
        }
    }
}
