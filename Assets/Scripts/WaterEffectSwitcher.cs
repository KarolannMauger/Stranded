using UnityEngine;

public static class WaterEffectSwitcher
{
    public static void Apply(string waterId)
    {
        switch (waterId)
        {
            case WaterIds.Good1:
                //TODO
                Debug.Log("Water Good1 effect applied");
                break;

            case WaterIds.Toxic1:
                //TODO
                Debug.Log("Water Toxic1 effect applied");
                break;

            default:
                Debug.LogWarning($"WaterEffectSwitcher: Id inconnu '{waterId}'");
                break;
        }
    }
}
