using UnityEngine;

public class VitalBarsController : MonoBehaviour
{
    // References to the individual bars
    private EnergyBar energyBar;
    private ThirstBar thirstBar;
    private HungerBar hungerBar;

    private Vector3 lastPosition;
    private float totalDistance;

    private void Start()
    {
        // Grab component references from the same GameObject
        energyBar = GetComponent<EnergyBar>();
        thirstBar = GetComponent<ThirstBar>();
        hungerBar = GetComponent<HungerBar>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        // Track player movement distance
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > 0.01f)
        {
            totalDistance += distanceMoved;
            thirstBar?.OnPlayerMoved(distanceMoved);
            hungerBar?.OnPlayerMoved(distanceMoved);
        }

        // Update energy bar based on hunger and thirst percentages
        if (energyBar != null && thirstBar != null && hungerBar != null)
        {
            float hungerPercent = (hungerBar.hunger / hungerBar.maxHunger) * 100f;
            float thirstPercent = (thirstBar.thirst / thirstBar.maxThirst) * 100f;

            energyBar.UpdateEnergy(hungerPercent, thirstPercent);
        }

        lastPosition = transform.position;
    }
}
