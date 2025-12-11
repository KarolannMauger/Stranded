using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        if (PlayerRespawnManager.Instance != null &&
            PlayerRespawnManager.Instance.HasSavedData())
        {
            transform.position = PlayerRespawnManager.Instance.GetSavedPosition();
        }
    }
}
