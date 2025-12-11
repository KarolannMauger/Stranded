using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerRespawnManager.Instance != null)
            {
                PlayerRespawnManager.Instance.RespawnPlayer();
            }
        });
    }
}
