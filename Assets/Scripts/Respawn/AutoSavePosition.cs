using UnityEngine;

public class AutoSavePosition : MonoBehaviour
{
    // Periodically saves this transform position for respawn
    private float saveInterval = 5f;
    private float timer = 0f;

    private void Update()
    {
        // Skip if the respawn manager is missing
        if (PlayerRespawnManager.Instance == null)
            return;

        timer += Time.deltaTime;

        if (timer >= saveInterval)
        {
            // Record the latest checkpoint and reset the timer
            PlayerRespawnManager.Instance.SaveCheckpoint(transform.position);
            timer = 0f;
        }
    }
}
