using UnityEngine;

public class AutoSavePosition : MonoBehaviour
{
    private float saveInterval = 5f;
    private float timer = 0f;

    private void Update()
    {
        if (PlayerRespawnManager.Instance == null)
            return;

        timer += Time.deltaTime;

        if (timer >= saveInterval)
        {
            PlayerRespawnManager.Instance.SaveCheckpoint(transform.position);
            timer = 0f;
        }
    }
}
