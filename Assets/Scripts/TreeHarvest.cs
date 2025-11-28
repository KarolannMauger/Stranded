using UnityEngine;
using System.Collections;

public class TreeHarvest : MonoBehaviour
{
    [Header("Harvest Settings")]
    public int maxHealth = 10;
    public float interactionDistance = 3f;
    public GameObject interactionUI;

    [Header("Fall Settings")]
    [Tooltip("Duree de la chute en secondes")]
    public float fallDuration = 1.0f;

    [Tooltip("Angle total de chute en degres")]
    public float fallAngle = 90f;

    [Header("Dust FX")]
    [Tooltip("Prefab du ParticleSystem de poussiere")]
    public ParticleSystem dustPrefab;

    [Tooltip("Décalage vertical (Y) par rapport au sol pour la poussiere")]
    public float dustGroundOffsetY = 0.05f;

    [Tooltip("Nombre de points de poussiere le long de larbre")]
    public int dustSegments = 4;

    private int currentHealth;
    private Transform _player;
    private TreeRuntimeInstance _runtime;

    private bool isFalling = false;
    private Quaternion startRot;
    private Quaternion targetRot;

    private bool dustPlayed = false;

    private float treeLength = 5f;
    private Vector3 fallDirection = Vector3.forward;
    private Vector3 bottomLocalPos;

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

        var rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            treeLength = rend.bounds.size.y;

            Vector3 bottomWorld = new Vector3(
                rend.bounds.center.x,
                rend.bounds.min.y,
                rend.bounds.center.z
            );

            bottomLocalPos = transform.InverseTransformPoint(bottomWorld);
        }
        else
        {
            treeLength = 5f;
            bottomLocalPos = Vector3.zero;
        }
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
        dustPlayed = false;

        Vector3 away = transform.forward;
        if (damageSource != null)
        {
            away = (transform.position - damageSource.position);
            away.y = 0f;
            if (away.sqrMagnitude < 0.001f)
                away = transform.forward;
            away.Normalize();
        }

        fallDirection = away; 

        
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

            if (!dustPlayed && k >= 0.95f)
            {
                PlayDust();
                dustPlayed = true;
            }

            yield return null;
        }

        transform.rotation = targetRot;

        if (!dustPlayed)
        {
            PlayDust();
            dustPlayed = true;
        }

        Harvest();
    }

    private void PlayDust()
    {
        if (dustPrefab == null) return;

        if (dustSegments <= 0)
            dustSegments = 1;

        Vector3 dir = fallDirection;
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;

        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f)
            dir = Vector3.forward;
        dir.Normalize();

        Vector3 bottomWorld = transform.TransformPoint(bottomLocalPos);
        bottomWorld.y += dustGroundOffsetY;

        for (int i = 0; i < dustSegments; i++)
        {
            float f = (i + 0.5f) / dustSegments;

            Vector3 pos = bottomWorld + dir * (treeLength * f);

            ParticleSystem dust = Instantiate(dustPrefab, pos, Quaternion.identity);
            dust.Play();

            var main = dust.main;
            Destroy(dust.gameObject, main.duration + main.startLifetime.constantMax);
        }
    }

    private void Harvest()
    {
        if (GameInventory.Instance != null)
        {
            GameInventory.Instance.AddWood(1);
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
#endif
}
