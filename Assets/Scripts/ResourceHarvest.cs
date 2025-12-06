using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ResourceHarvest : MonoBehaviour
{
    [Header("Harvest Settings")]
    [Tooltip("Distance max pour pouvoir recolter")]
    public float interactionDistance = 2f;

    [Tooltip("UI a afficher quand le joueur est a portee (facultatif)")]
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

        if (GameInventory.Instance != null)
        {
            if (mushInfo != null)
            {
                GameInventory.Instance.EatMushroom(mushInfo.mushroomId);
                MushroomEffectSwitcher.Apply(mushInfo.mushroomId);
                Debug.Log($"[ResourceHarvest] Champignon recolte : {mushInfo.mushroomId}");
            }
            else
            {
                GameInventory.Instance.AddRock(1);
                Debug.Log("[ResourceHarvest] Roche recoltee (+1 rock)");
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
