using UnityEngine;

public class MushroomInfo : MonoBehaviour
{
    // Identifier used to map this mushroom to its effect
    [Tooltip("Id of the mushroom type (e.g., Toxic1, Good1, Toxic2...)")]
    public string mushroomId = MushroomIds.Good1;
}
