using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private SceneController _sceneController; // manage les scenes avec ceci pour fadeout

    [Header("Nom de la scène de jeu")]
    public string gameSceneName = "SampleScene";
    public string menuSceneName = "Scenes/StartScene";

    void Start()
    {
        // S'assurer que le curseur est visible et déverrouillé dans les menus
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Assignée au bouton "Jouer"
    public void OnPlay()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            Cursor.visible = false;
            _sceneController.LoadScene(gameSceneName);
        }
    }

    // Assignée au bouton "Menu"
    public void OnMenu()
    {
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            // S'assurer que le curseur est visible avant de charger le menu
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _sceneController.LoadScene(menuSceneName);
        }
    }

    //Assignée au bouton "Quitter"
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}