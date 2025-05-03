using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    void Awake()
    {
        // Ensure this GameObject persists across scene loads
        DontDestroyOnLoad(gameObject);
    }
}
