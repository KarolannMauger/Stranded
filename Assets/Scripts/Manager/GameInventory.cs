using UnityEngine;

public class GameInventory : MonoBehaviour
{
    // Global inventory instance to track player resources
    public static GameInventory Instance { get; private set; }

    [Header("Ressources")]
    public int woodCount;
    public int rockCount;

    [Header("Champignons")]
    public string lastMushroomId;
    public int totalMushroomsEaten;

    
    public static string LastMushroomId => Instance != null ? Instance.lastMushroomId : null;

    private void Awake()
    {
        // Enforce singleton pattern across scenes
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
        // Track gathered wood for crafting or missions
        woodCount += amount;
        Debug.Log($"[Inventory] Wood +{amount} → total = {woodCount}");
    }

    public void AddRock(int amount)
    {
        // Track gathered rocks for crafting or missions
        rockCount += amount;
        Debug.Log($"[Inventory] Rocks +{amount} → total = {rockCount}");
    }

    public void EatMushroom(string mushroomId)
    {
        // Remember the last mushroom consumed
        lastMushroomId = mushroomId;
        totalMushroomsEaten++;

        Debug.Log($"[Inventory] Mushroom eaten: {mushroomId} | Total mushrooms = {totalMushroomsEaten}");
    }
}
