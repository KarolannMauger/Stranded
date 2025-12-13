using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ResourceHarvest : MonoBehaviour
{
    // Handles player interaction to collect mushrooms or rocks
    [Header("Harvest Settings")]
    [Tooltip("Maximum distance for harvesting")]
    public float interactionDistance = 2f;

    [Tooltip("UI to display when the player is in range (optional)")]
    public GameObject interactionUI;

    private Transform _player;
    private TreeRuntimeInstance _runtime;
    private bool _isCollecting = false;

    void Start()
    {
        _runtime = GetComponent<TreeRuntimeInstance>();

        // Cache player transform for distance checks
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogError("[ResourceHarvest] No object with tag 'Player' found.");
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        // Do nothing if no player or harvest already in progress
        if (_player == null || _isCollecting)
            return;

        float dist = Vector3.Distance(_player.position, transform.position);
        bool inRange = dist <= interactionDistance;

        if (interactionUI != null)
            interactionUI.SetActive(inRange);

        if (!inRange || Keyboard.current == null)
            return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log($"[ResourceHarvest] F pressed on {name}, dist={dist}");
            StartCoroutine(HarvestRoutine());
        }
    }

    private IEnumerator HarvestRoutine()
    {
        _isCollecting = true;

        // Small delay so the input event and animation can settle
        yield return new WaitForSeconds(0.05f);

        MushroomInfo mushInfo = GetComponent<MushroomInfo>();

        if (GameInventory.Instance != null)
        {
            if (mushInfo != null)
            {
                // Apply mushroom-specific effects and inventory tracking
                GameInventory.Instance.EatMushroom(mushInfo.mushroomId);
                MushroomEffectSwitcher.Apply(mushInfo.mushroomId);
                Debug.Log($"[ResourceHarvest] Mushroom harvested: {mushInfo.mushroomId}");
            }
            else
            {
                // Default fallback: count as a rock resource
                GameInventory.Instance.AddRock(1);
                Debug.Log("[ResourceHarvest] Rocks harvested (+1 rock)");
            }
        }

        if (_runtime != null && _runtime.manager != null && _runtime.treeIndex >= 0)
        {
            _runtime.manager.HarvestTree(_runtime.treeIndex);
        }
        else
        {
            Destroy(gameObject);
        }

        _isCollecting = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
#endif
}
