using UnityEngine;
using System.Collections;

public class TreeHarvest : MonoBehaviour
{
    [Header("Harvest Settings")]
    public int maxHealth = 10;
    public float interactionDistance = 3f;
    public GameObject interactionUI;

    [Header("Fall Settings")]
    [Tooltip("Durée de la chute en secondes")]
    public float fallDuration = 1.0f;

    [Tooltip("Angle total de chute en degrés")]
    public float fallAngle = 90f;

    private int currentHealth;
    private Transform _player;
    private TreeRuntimeInstance _runtime;

    
    private bool isFalling = false;
    private Quaternion startRot;
    private Quaternion targetRot;

    void Start()
    {
        currentHealth = maxHealth;

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
            interactionUI.SetActive(inRange && currentHealth > 0);
    }

    public void TakeDamage(int amount, Transform damageSource)
    {
        if (currentHealth <= 0 || isFalling)
            return;

        if (damageSource != null)
        {
            float dist = Vector3.Distance(damageSource.position, transform.position);
            if (dist > interactionDistance + 0.5f)
                return;
        }

        currentHealth -= amount;
        Debug.Log($"[TreeHarvest] {name} subit {amount} degats. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            
            StartFall(damageSource);
        }
    }

    private void StartFall(Transform damageSource)
    {
        isFalling = true;

        
        Vector3 away = transform.forward;
        if (damageSource != null)
        {
            away = (transform.position - damageSource.position);
            away.y = 0f;
            if (away.sqrMagnitude < 0.001f)
                away = transform.forward;
            away.Normalize();
        }

        
        Vector3 fallAxis = Vector3.Cross(Vector3.up, away).normalized;
        if (fallAxis.sqrMagnitude < 0.001f)
            fallAxis = transform.right;

        startRot = transform.rotation;
        targetRot = Quaternion.AngleAxis(fallAngle, fallAxis) * startRot;

        StartCoroutine(FallThenHarvest());
    }

    private IEnumerator FallThenHarvest()
    {
        float t = 0f;

        while (t < fallDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fallDuration);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, k);
            yield return null;
        }

        Harvest();
    }

    private void Harvest()
    {
        Debug.Log($"[TreeHarvest] Arbre recolte/detruit: {name}");

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
