using UnityEngine;

public static class MushroomEffectSwitcher
{
    public static void Apply(string mushroomId)
    {
        // Find the player to apply hunger effects
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("MushroomEffectSwitcher: Player not found in the scene.");
            return;
        }

        HungerBar hungerBar = player.GetComponent<HungerBar>();

        if (hungerBar == null)
        {
            Debug.LogError("MushroomEffectSwitcher: HungerBar component not found on Player.");
            return;
        }

        switch (mushroomId)
        {
            case MushroomIds.Good1:
                hungerBar.ConsumeMushroom(isToxic: false);
                Debug.Log("Mushroom Good1 effect applied");
                break;

            case MushroomIds.Good2:
                hungerBar.ConsumeMushroom(isToxic: false);
                Debug.Log("Mushroom Good2 effect applied");
                break;

            case MushroomIds.Toxic1:
                hungerBar.ConsumeMushroom(isToxic: true);
                Debug.Log("Mushroom Toxic1 effect applied");
                break;

            case MushroomIds.Toxic2:
                hungerBar.ConsumeMushroom(isToxic: true);
                Debug.Log("Mushroom Toxic2 effect applied");
                break;

            default:
                // Unknown id: warn and skip applying an effect
                Debug.LogWarning($"MushroomEffectSwitcher: Id unknown '{mushroomId}'");
                break;
        }
    }
}
