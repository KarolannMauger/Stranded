using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    // Tracks hunger value and updates the UI slider
    public float hunger = 100f;
    public float maxHunger = 100f;
    public Slider hungerSlider;

    private float distanceAccumulator = 0f;
    private float metersPerLoss = 75f;

    private void Awake()
    {
        // Find the UI slider in the scene if not assigned
        hungerSlider = GameObject.Find("HungerBar")?.GetComponent<Slider>();
    }

    private void Start()
    {
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = hunger;
    }

    public void OnPlayerMoved(float metersMoved)
    {
        // Consume hunger based on distance traveled
        distanceAccumulator += metersMoved;

        if (distanceAccumulator >= metersPerLoss)
        {
            float loss = Mathf.Floor(distanceAccumulator / metersPerLoss);
            hunger -= loss;
            distanceAccumulator -= loss * metersPerLoss;
            hunger = Mathf.Clamp(hunger, 0f, maxHunger);
            UpdateUI();
        }
    }
    public void ConsumeMushroom(bool isToxic)
    {
        // Good mushrooms restore, toxic ones reduce hunger
        hunger += isToxic ? -20f : 10f;
        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        UpdateUI();
    }

    private void UpdateUI()
    {
        hungerSlider.value = hunger;
    }
}
