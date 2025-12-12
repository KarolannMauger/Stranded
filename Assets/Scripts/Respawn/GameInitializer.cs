using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        // Restore position if a checkpoint was saved in a previous session
        if (PlayerRespawnManager.Instance != null &&
            PlayerRespawnManager.Instance.HasSavedData())
        {
            transform.position = PlayerRespawnManager.Instance.GetSavedPosition();
        }
    }
}
