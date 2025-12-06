using UnityEngine;
using UnityEngine.InputSystem;

public class WaterZone : MonoBehaviour
{
    [Header("Ocean Settings")]
    public float drinkCooldown = 1f;
    public GameObject interactionUI;

    private Collider _collider;
    private bool _playerInside = false;
    private float _nextDrinkTime = 0f;

    private const string OCEAN_WATER_ID = "Water_Toxic1";

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null)
        {
            Debug.LogWarning($"[OCEAN] Aucun collider trouve sur '{name}', WaterZone desactive.");
            enabled = false;
        }
    }

    private void Start()
    {
        if (!enabled)
            return;

        if (_collider != null && !_collider.isTrigger)
        {
            Debug.LogWarning($"[OCEAN] Collider non trigger sur '{name}', WaterZone desactive (reserve aux triggers pour l'ocean).");
            enabled = false;
            return;
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);

        Debug.Log($"[OCEAN] WaterZone actif sur: {name}");
    }

    private void Update()
    {
        if (!_playerInside || Keyboard.current == null)
            return;

        if (Time.time < _nextDrinkTime)
            return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            _nextDrinkTime = Time.time + drinkCooldown;

            Debug.Log($"[OCEAN] Drink from OCEAN zone '{name}' → {OCEAN_WATER_ID}");
            WaterEffectSwitcher.Apply(OCEAN_WATER_ID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[OCEAN] OnTriggerEnter on '{name}' with '{other.name}'");

        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            if (interactionUI != null) interactionUI.SetActive(true);
            Debug.Log($"[OCEAN] PLAYER ENTER ocean zone: {name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[OCEAN] OnTriggerExit on '{name}' with '{other.name}'");

        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            if (interactionUI != null) interactionUI.SetActive(false);
            Debug.Log($"[OCEAN] PLAYER EXIT ocean zone: {name}");
        }
    }
}
