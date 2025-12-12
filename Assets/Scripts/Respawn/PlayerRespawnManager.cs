using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawnManager : MonoBehaviour
{
    // Singleton that stores the latest checkpoint and reloads the scene
    public static PlayerRespawnManager Instance;

    private Vector3 lastSavedPosition;
    private bool hasSavedData = false;

    public string mainGameSceneName = "SampleScene";

    private void Awake()
    {
        // Persist across scenes; destroy duplicates
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckpoint(Vector3 playerPosition)
    {
        // Remember the most recent checkpoint location
        lastSavedPosition = playerPosition;
        hasSavedData = true;
    }

    public void RespawnPlayer()
    {
        // Reload the main scene so GameInitializer can place the player
        if (!hasSavedData)
            return;

        SceneManager.LoadScene(mainGameSceneName);
    }

    public Vector3 GetSavedPosition()
    {
        return lastSavedPosition;
    }

    public bool HasSavedData()
    {
        return hasSavedData;
    }
}
