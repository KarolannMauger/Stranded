using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnergyBar : MonoBehaviour
{
    public float energy = 100f;
    public float maxEnergy = 100f;
    public Slider energySlider;

    private float energyDrainRate = 0.5f;
    private float energyRegenRate = 0.5f;

    private bool isGameOver = false;

    private void Awake()
    {
        energySlider = GameObject.Find("EnergyBar")?.GetComponent<Slider>();
    }

    private void Start()
    {
        // Initialize UI
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = energy;
        }
    }

    public void UpdateEnergy(float hungerPercent, float thirstPercent)
    {
        if (isGameOver)
            return;

        // Adjust energy based on hunger and thirst percentages
        if (thirstPercent <= 0f)
            energy -= 4f * Time.deltaTime;
        else if (hungerPercent <= 0f)
            energy -= 2f * Time.deltaTime;
        else if (hungerPercent >= 80f && thirstPercent >= 80f)
            energy += energyRegenRate * Time.deltaTime;
        else
            energy -= energyDrainRate * Time.deltaTime;

        energy = Mathf.Clamp(energy, 0f, maxEnergy);
        UpdateUI();
        CheckGameOver();
    }

    private void UpdateUI()
    {
        if (energySlider != null)
            energySlider.value = energy;
    }

    private void CheckGameOver()
    {
        // Trigger game over if energy reaches zero
        if (energy <= 0f && !isGameOver)
        {
            isGameOver = true;
            SceneManager.LoadScene("GameOverScene");
        }
    }
}