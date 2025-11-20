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
            Debug.Log("[ResourceHarvest] F press detected → " + name);
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
            }
            else
            {
                GameInventory.Instance.AddRock(1);
            }
        }

        if (_runtime != null && _runtime.manager != null)
        {
            _runtime.manager.HarvestTree(_runtime.treeIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
#endif
}
