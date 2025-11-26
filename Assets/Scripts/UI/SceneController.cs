using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // source: https://www.youtube.com/watch?v=vkOhefMbrFg

    [SerializeField]
    private float _sceneFadeDuration;

    private SceneFade _sceneFade;

    // récupère composant scenefade
    private void Awake()
    {
        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    // lance fadein au lancement du jeu
    private IEnumerator Start()
    {
        yield return _sceneFade.FadeInCoroutine(_sceneFadeDuration);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneFade.FadeOutCoroutine(_sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
