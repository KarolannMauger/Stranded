using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SceneFade : MonoBehaviour
{
    // source: https://www.youtube.com/watch?v=vkOhefMbrFg
    private Image _sceneFadeImage;

    private void Awake()
    {
        _sceneFadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1); // Couleur de début (opacité nulle)
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0); // Couleur de fin (opacité totale)

        yield return FadeCoroutine(startColor, targetColor, duration); // fade out
        gameObject.SetActive(false); // désactive l'objet
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0); // Couleur de fin (opacité totale)
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1); // Couleur de début (opacité nulle)

        gameObject.SetActive(true); // active l'objet
        yield return FadeCoroutine(startColor, targetColor, duration); // fade in
    }

    private IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            _sceneFadeImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
