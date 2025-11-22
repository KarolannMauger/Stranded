using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private SceneController _sceneController; // manage les scenes avec ceci pour fadeout

    [Header("Nom de la scène de jeu")]
    public string gameSceneName = "Level";
    public string menuSceneName = "Start";
    // Assignée au bouton "Jouer"
    public void OnPlay()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            _sceneController.LoadScene(gameSceneName);
        }
    }

    // Assignée au bouton "Menu"
    public void OnMenu()
    {
        if (!string.IsNullOrEmpty(menuSceneName))
        {
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