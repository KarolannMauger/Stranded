using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SceneFade : MonoBehaviour
{
    // source: https://www.youtube.com/watch?v=vkOhefMbrFg
    private Image _sceneFadeImage;

    private void Awake()
    {
        // Cache the Image component used for fading
        _sceneFadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1); // Fully opaque
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0); // Fully transparent

        yield return FadeCoroutine(startColor, targetColor, duration); // Fade out overlay
        gameObject.SetActive(false); // Disable overlay when done
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0); // Fully transparent
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1); // Fully opaque

        gameObject.SetActive(true); // Enable overlay before fade
        yield return FadeCoroutine(startColor, targetColor, duration); // Fade in overlay
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
