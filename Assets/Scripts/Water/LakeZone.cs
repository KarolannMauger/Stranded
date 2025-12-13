using UnityEngine;
using UnityEngine.InputSystem;

public class LakeZone : MonoBehaviour
{
    [Header("Lake Settings")]
    public string waterId = "Water_Good1";
    public float drinkCooldown = 1f;
    public GameObject interactionUI;

    private bool _playerInside = false;
    private float _nextDrinkTime = 0f;

    private void Start()
    {
        // Hide prompt by default
        if (interactionUI != null)
            interactionUI.SetActive(false);
        
        Debug.Log($"[LAKE] LakeZone initialized on: {name}, waterId={waterId}");
    }

    private void Update()
    {
        // Do nothing if the player is not in the zone or no keyboard input
        if (!_playerInside || Keyboard.current == null)
            return;

        if (Time.time < _nextDrinkTime)
            return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            _nextDrinkTime = Time.time + drinkCooldown;

            Debug.Log($"[LAKE] Drink from LAKE zone '{name}' → {waterId}");
            WaterEffectSwitcher.Apply(waterId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[LAKE] OnTriggerEnter '{name}' with '{other.name}'");

        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            if (interactionUI != null)
                interactionUI.SetActive(true);
            Debug.Log($"[LAKE] PLAYER ENTER lake: {name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[LAKE] OnTriggerExit '{name}' with '{other.name}'");

        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);
            Debug.Log($"[LAKE] PLAYER EXIT lake: {name}");
        }
    }
}
