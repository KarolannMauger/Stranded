using UnityEngine;
using UnityEngine.InputSystem; // Nouveau Input System IMPORTANT

public class TreeHarvest : MonoBehaviour
{
    public float interactionDistance = 3f;
    public GameObject interactionUI;

    private Transform _player;
    private TreeRuntimeInstance _runtime;

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
            Debug.LogError("[TreeHarvest] Aucun objet avec le tag 'Player' trouve.");
        }

        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(_player.position, transform.position);
        bool inRange = dist <= interactionDistance;

        if (interactionUI != null)
            interactionUI.SetActive(inRange);

        if (!inRange)
            return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Harvest();
        }
    }

    private void Harvest()
    {
        Debug.Log($"[TreeHarvest] Arbre recolte: {name}");

        if (_runtime != null && _runtime.manager != null)
        {
            _runtime.manager.HarvestTree(_runtime.treeIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
