using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private LevelCompletion levelCompletion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (levelCompletion != null && levelCompletion.AreAllEnemiesDefeated() && levelCompletion.KeyCollectedStatus)
            {
                levelCompletion.LevelComplete(); //
            }
        }
    }
}
