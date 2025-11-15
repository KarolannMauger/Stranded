using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    [Header("References")]
    public Animator axeAnimator;
    public Transform cameraTransform; 

    [Header("Hit Settings")]
    public float hitDistance = 3f;
    public int damagePerHit = 1;
    public LayerMask hitLayers = ~0;

    [Header("Timing")]
    public float attackCooldown = 0.35f;

    private bool canAttack = true;

    public void DoAttack()
    {
        if (!canAttack)
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
            Debug.LogWarning("[AxeAttack] CameraTransform non assigne.");
            return;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, hitDistance, hitLayers))
        {

            TreeHarvest tree = hit.collider.GetComponentInParent<TreeHarvest>();
            if (tree != null)
            {
                tree.TakeDamage(damagePerHit, cameraTransform);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
