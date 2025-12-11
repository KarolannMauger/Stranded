using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawnManager : MonoBehaviour
{
    public static PlayerRespawnManager Instance;

    private Vector3 lastSavedPosition;
    private bool hasSavedData = false;

    public string mainGameSceneName = "SampleScene";

    private void Awake()
    {
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
        lastSavedPosition = playerPosition;
        hasSavedData = true;
    }

    public void RespawnPlayer()
    {
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
