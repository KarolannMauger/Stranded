using UnityEngine;

public static class MushroomEffectSwitcher
{
    public static void Apply(string mushroomId)
    {
        switch (mushroomId)
        {
            case MushroomIds.Good1:
                //TODO
                //Example : PlayerStats.Instance.AddHealth(15);
                Debug.Log("Mushroom Good1 effect applied");
                break;

            case MushroomIds.Good2:
                //TODO
                Debug.Log("Mushroom Good2 effect applied");
                break;

            case MushroomIds.Toxic1:
                //TODO
                Debug.Log("Mushroom Toxic1 effect applied");
                break;

            case MushroomIds.Toxic2:
                //TODO
                Debug.Log("Mushroom Toxic2 effect applied");
                break;

            default:
                Debug.LogWarning($"MushroomEffectSwitcher: Id inconnu '{mushroomId}'");
                break;
        }
    }
}
