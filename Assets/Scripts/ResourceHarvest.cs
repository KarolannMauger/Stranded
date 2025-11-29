using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ResourceHarvest : MonoBehaviour
{
    [Header("Harvest Settings")]
    public float interactionDistance = 2f;
    public GameObject interactionUI;

    private Transform _player;
    private TreeRuntimeInstance _runtime;
    private bool _isCollecting = false;

    void Start()
    {
        _runtime = GetComponent<TreeRuntimeInstance>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
        else
        {
            Debug.LogError("[ResourceHarvest] Aucun objet avec le tag 'Player' trouve.");
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
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

        yield return new WaitForSeconds(0.05f);

        MushroomInfo mushInfo = GetComponent<MushroomInfo>();
        WaterInfo    waterInfo = GetComponent<WaterInfo>();

        if (GameInventory.Instance != null)
        {
            if (mushInfo != null)
            {
                GameInventory.Instance.EatMushroom(mushInfo.mushroomId);
            }
            else if (waterInfo == null)
            {
                GameInventory.Instance.AddRock(1);
            }
        }

        if (mushInfo != null)
        {
            MushroomEffectSwitcher.Apply(mushInfo.mushroomId);
        }
        else if (waterInfo != null)
        {
            WaterEffectSwitcher.Apply(waterInfo.waterId);
            Debug.Log($"[ResourceHarvest] Eau récoltée : {waterInfo.waterId}");
        }

        if (_runtime != null && _runtime.manager != null && _runtime.treeIndex >= 0)
        {
            _runtime.manager.HarvestTree(_runtime.treeIndex);
        }
        else
        {
            if (waterInfo == null)
            {
                Destroy(gameObject);
            }
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
