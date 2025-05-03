using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    [SerializeField] private LevelCompletion levelCompletion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (levelCompletion != null)
            {
                levelCompletion.KeyCollected();
            }
            gameObject.SetActive(false); // Deactivate the key collectible
        }
    }
}