using UnityEngine;

public static class WaterEffectSwitcher
{
    public static void Apply(string waterId)
    {
        // Locate the player to apply thirst effects
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("WaterEffectSwitcher: Player not found in the scene.");
            return;
        }

        ThirstBar thirstBar = player.GetComponent<ThirstBar>();

        if (thirstBar == null)
        {
            Debug.LogError("WaterEffectSwitcher: ThirstBar component not found on Player.");
            return;
        }

        switch (waterId)
        {
            case WaterIds.Good1:
                thirstBar.DrinkWater(isFiltered: true);
                Debug.Log("Water Good1 effect applied");
                break;

            case WaterIds.Toxic1:
                thirstBar.DrinkWater(isFiltered: false);
                Debug.Log("Water Toxic1 effect applied");
                break;

            default:
                // Unknown id: warn and skip applying an effect
                Debug.LogWarning($"WaterEffectSwitcher: Id unknown '{waterId}'");
                break;
        }
    }
}
