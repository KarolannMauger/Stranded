using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{
    public float thirst = 100f;
    public float maxThirst = 100f;
    public Slider thirstSlider;

    private float distanceAccumulator = 0f;
    private float metersPerLoss = 50f;

    private void Awake()
    {
        thirstSlider = GameObject.Find("ThirstBar")?.GetComponent<Slider>();
    }

    private void Start()
    {
        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = thirst;
    }

    public void OnPlayerMoved(float metersMoved)
    {
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
        thirst += isFiltered ? 15f : 5f;
        thirst = Mathf.Clamp(thirst, 0f, maxThirst);
        UpdateUI();
    }

    private void UpdateUI()
    {
        thirstSlider.value = thirst;
    }
}
