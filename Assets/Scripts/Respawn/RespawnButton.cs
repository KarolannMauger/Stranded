using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    private void Start()
    {
        // Wire the UI button to trigger a scene reload via the manager
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerRespawnManager.Instance != null)
            {
                PlayerRespawnManager.Instance.RespawnPlayer();
            }
        });
    }
}
