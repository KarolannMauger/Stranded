using UnityEngine;
using UnityEngine.InputSystem;

public class AxeAttackOnKey : MonoBehaviour
{
    [Header("References")]
    public Animator axeAnimator;
    public Transform cameraTransform;

    [Header("Hit Settings")]
    public float hitDistance = 3f;
    public int damagePerHit = 1;
    public LayerMask hitLayers;

    [Header("Timing")]
    public float attackCooldown = 0.35f;

    private bool canAttack = true;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || !canAttack)
            return;

        canAttack = false;

        if (axeAnimator != null)
        {
            axeAnimator.SetTrigger("Attack");
        }

        TryHitTree();

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void TryHitTree()
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("[AxeAttackOnKey] CameraTransform non assigne.");
            return;
        }

        if (Physics.Raycast(cameraTransform.position,
                            cameraTransform.forward,
                            out RaycastHit hit,
                            hitDistance,
                            hitLayers))
        {
            TreeHarvest tree = hit.collider.GetComponentInParent<TreeHarvest>();

            if (tree != null)
            {
                tree.TakeDamage(damagePerHit, cameraTransform);
                Debug.Log($"[AxeAttackOnKey] Touche arbre: {tree.name}");
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
