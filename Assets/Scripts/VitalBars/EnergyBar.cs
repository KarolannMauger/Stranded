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
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = energy;
        }
    }

    public void UpdateEnergy(
        float hungerPercent,
        float thirstPercent,
        bool isDayTime
    )
    {
        if (isGameOver)
            return;

        if (thirstPercent <= 0f)
        {
            energy -= 4f * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            UpdateUI();
            CheckGameOver();
            return;
        }
        else if (hungerPercent <= 0f)
        {
            energy -= 2f * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            UpdateUI();
            CheckGameOver();
            return;
        }

        if (hungerPercent >= 80f && thirstPercent >= 80f)
        {
            energy += energyRegenRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            UpdateUI();
        }
        else
        {
            energy -= energyDrainRate * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0f, maxEnergy);
            UpdateUI();
            CheckGameOver();
        }
    }

    private void UpdateUI()
    {
        if (energySlider != null)
            energySlider.value = energy;
    }

    private void CheckGameOver()
    {
        if (energy <= 0f && !isGameOver)
        {
            isGameOver = true;
            SceneManager.LoadScene("GameOverScene");
        }
    }
}