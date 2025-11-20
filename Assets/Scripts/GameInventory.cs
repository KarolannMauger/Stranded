using UnityEngine;

public class GameInventory : MonoBehaviour
{
    public static GameInventory Instance { get; private set; }

    [Header("Ressources")]
    public int woodCount;
    public int rockCount;

    [Header("Champignons (pour futurs effets)")]
    public string lastMushroomId;
    public int totalMushroomsEaten;

    
    public static string LastMushroomId => Instance != null ? Instance.lastMushroomId : null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddWood(int amount)
    {
        woodCount += amount;
        Debug.Log($"[Inventory] Bois +{amount} → total = {woodCount}");
    }

    public void AddRock(int amount)
    {
        rockCount += amount;
        Debug.Log($"[Inventory] Roche +{amount} → total = {rockCount}");
    }

    public void EatMushroom(string mushroomId)
    {
        lastMushroomId = mushroomId;
        totalMushroomsEaten++;

        Debug.Log($"[Inventory] Champignon mange : {mushroomId} | Total champignons = {totalMushroomsEaten}");
    }
}
