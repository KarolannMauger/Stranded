using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private SceneController _sceneController; // Handles scene transitions with fade

    [Header("Name of the game scene")]
    public string gameSceneName = "SampleScene";
    public string menuSceneName = "Scenes/StartScene";

    void Start()
    {
        // Keep the cursor visible and unlocked in menus
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Invoked by the Play button to start the game
    public void OnPlay()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            BeginningTrigger.ResetTrigger();

            Cursor.visible = false;
            _sceneController.LoadScene(gameSceneName);
        }
    }

    public void OnRespawn()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            _sceneController.LoadScene(gameSceneName);
        }
    }

    // Invoked by the Menu button to return to the main menu
    public void OnMenu()
    {
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            // Ensure the cursor is visible before loading the menu
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _sceneController.LoadScene(menuSceneName);
        }
    }

    // Invoked by the Quit button
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
