using UnityEngine;

public class VitalBarsController : MonoBehaviour
{
    private EnergyBar energyBar;
    private ThirstBar thirstBar;
    private HungerBar hungerBar;
    private Vector3 lastPosition;
    private float totalDistance = 0f;
    public bool isDayTime = true;

    private void Start()
    {
        energyBar = GetComponent<EnergyBar>();
        thirstBar = GetComponent<ThirstBar>();
        hungerBar = GetComponent<HungerBar>();
        lastPosition = transform.position;

        if (energyBar == null)
            Debug.LogError("EnergyBar component not found!");
        if (thirstBar == null)
            Debug.LogError("ThirstBar component not found!");
        if (hungerBar == null)
            Debug.LogError("HungerBar component not found!");
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > 0.01f)
        {
            totalDistance += distanceMoved;

            if (thirstBar != null)
                thirstBar.OnPlayerMoved(distanceMoved);
            if (hungerBar != null)
                hungerBar.OnPlayerMoved(distanceMoved);
        }

        if (energyBar != null && thirstBar != null && hungerBar != null)
        {
            float hungerPercent = (hungerBar.hunger / hungerBar.maxHunger) * 100f;
            float thirstPercent = (thirstBar.thirst / thirstBar.maxThirst) * 100f;
            energyBar.UpdateEnergy(hungerPercent, thirstPercent, isDayTime);
        }

        lastPosition = transform.position;
    }
}
