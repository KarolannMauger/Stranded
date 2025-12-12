using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    // Handles axe swing animation, raycast, and hit feedback
    [Header("References")]
    public Animator axeAnimator;
    public Transform cameraTransform;
    public AudioSource audioSource; 

    [Header("Hit Settings")]
    public float hitDistance = 3f;
    public int damagePerHit = 1;
    public LayerMask hitLayers = ~0;

    [Header("Timing")]
    public float attackCooldown = 0.35f;

     [Header("Audio")]
    public AudioClip hitSound;

    private bool canAttack = true;

    public void DoAttack()
    {
        // Prevent overlapping attacks while the cooldown is active
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
        // Ensure we have a valid camera to cast the ray from
        if (cameraTransform == null)
        {
            //Debug.LogWarning("[AxeAttack] CameraTransform not assigned.");
            return;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, hitDistance, hitLayers))
        {

            TreeHarvest tree = hit.collider.GetComponentInParent<TreeHarvest>();
            if (tree != null)
            {
                tree.TakeDamage(damagePerHit, cameraTransform);
                PlayHitSound();
            }
        }
    }
      private void PlayHitSound()
    {
        // Play feedback only when both components are configured
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            //Debug.LogWarning("[AxeAttack] AudioSource or hitSound not assigned.");
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
