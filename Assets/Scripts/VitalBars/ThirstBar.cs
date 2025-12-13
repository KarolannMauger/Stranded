using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{
    // Tracks thirst value and updates the UI slider
    public float thirst = 100f;
    public float maxThirst = 100f;
    public Slider thirstSlider;

    private float distanceAccumulator = 0f;
    private float metersPerLoss = 50f;

    private void Awake()
    {
        // Find the UI slider in the scene if not assigned
        thirstSlider = GameObject.Find("ThirstBar")?.GetComponent<Slider>();
    }

    private void Start()
    {
        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = thirst;
    }

    public void OnPlayerMoved(float metersMoved)
    {
        // Consume thirst based on distance traveled
        distanceAccumulator += metersMoved;

        if (distanceAccumulator >= metersPerLoss)
        {
            float loss = Mathf.Floor(distanceAccumulator / metersPerLoss);
            thirst -= loss;
            distanceAccumulator -= loss * metersPerLoss;
            thirst = Mathf.Clamp(thirst, 0f, maxThirst);
            UpdateUI();
        }
    }

    public void DrinkWater(bool isFiltered)
    {
        // Filtered water restores more thirst than unfiltered
        thirst += isFiltered ? 15f : 5f;
        thirst = Mathf.Clamp(thirst, 0f, maxThirst);
        UpdateUI();
    }

    private void UpdateUI()
    {
        thirstSlider.value = thirst;
    }
}
