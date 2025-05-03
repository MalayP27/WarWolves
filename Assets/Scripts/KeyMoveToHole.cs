using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyMoveToHole : MonoBehaviour
{
    [SerializeField] private GameObject treasure;  // Serialized field for the key to be moved
    [SerializeField] private Transform keyholePosition;  // Assign the KeyHole's transform in the Inspector
    [SerializeField] private float snapDistance = 0.5f;  // How close the key has to be to snap into place
    [SerializeField] private float moveSpeed = 1f;       // Speed of movement


    private bool isSolved = false;

    void Update()
    {
        if (isSolved) return;

        // Handle key movement using WASD
        Vector3 movement = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) // Move Up
        {
            movement.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) // Move Down
        {
            movement.y -= 1;
        }
        if (Input.GetKey(KeyCode.A)) // Move Left
        {
            movement.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) // Move Right
        {
            movement.x += 1;
        }

        // Normalize movement vector to make diagonal movement consistent
        movement.Normalize();

        // Apply movement to key position
        treasure.transform.position += movement * moveSpeed * Time.deltaTime;

        // Check if key is close enough to keyhole
        if (Vector3.Distance(treasure.transform.position, keyholePosition.position) <= snapDistance)
        {
            isSolved = true;
            treasure.transform.position = keyholePosition.position; // Snap key into place
            Invoke("ExitKeyChestPuzzle", 1f); // Delay to give feedback to player
        }
    }

    void ExitKeyChestPuzzle()
    {
        // Unload the KeyChestPuzzle scene
        SceneManager.UnloadSceneAsync("KeyChestPuzzle");

        // Re-enable the scripts that were disabled in the Level2 scene
        GameObject keyChestInteractionObject = GameObject.FindObjectOfType<KeyChestInteraction>()?.gameObject;
        if (keyChestInteractionObject != null)
        {
            KeyChestInteraction keyChestInteraction = keyChestInteractionObject.GetComponent<KeyChestInteraction>();
            if (keyChestInteraction != null)
            {
                keyChestInteraction.EnableDisabledScripts();

                // Enable the key object after solving the puzzle
                if (keyChestInteraction.treasure != null)
                {
                    keyChestInteraction.treasure.SetActive(true);
                }

                // Disable the KeyChestInteraction script after solving the puzzle
                keyChestInteraction.enabled = false;

                // Disable the interactionPromptText after solving the puzzle
                if (keyChestInteraction.interactionPromptText != null)
                {
                    keyChestInteraction.interactionPromptText.SetActive(false);
                }
            }
            
            keyChestInteraction.anim.SetTrigger("open");
        }
        
    }
}
