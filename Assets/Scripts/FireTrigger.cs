using UnityEngine;
using UnityEngine.InputSystem;

public class FireTrigger : MonoBehaviour
{
    [Header("Fire Object")]
    public GameObject fireObject;

    [Header("Input System")]
    public InputActionReference interactAction;

    [Header("UI Style")]
    public Font customFont;
    public int fontSize = 30;

    private bool enter;
    private bool fireBuilt = false;

    private void Start()
    {
        // hide firecamp
        if (fireObject != null)
        {
            fireObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Debug.Log("[FireTrigger] OnEnable called");
        
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteractPerformed;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteractPerformed;
            interactAction.action.Disable();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        
        if (!enter || fireBuilt) return;

        // verify mission and ressources
        if (MissionManager.Instance != null)
        {
            
            if (MissionManager.Instance.currentMission == MissionManager.MissionState.BuildFire)
            {
                BuildFire();
            }
        }
    }

    private void BuildFire()
    {
        fireBuilt = true;

        // activer le feu visuel
        if (fireObject != null)
        {
            fireObject.SetActive(true);
        }

        // alerte MissionManager
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnFireBuilt();
        }
    }


    // prompt player to press key if correct mission going on
    private void OnGUI()
    {
        if (enter && !fireBuilt && 
            MissionManager.Instance != null && 
            MissionManager.Instance.currentMission == MissionManager.MissionState.BuildFire)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = fontSize;
            style.alignment = TextAnchor.MiddleCenter;

            if (customFont != null)
            {
                style.font = customFont;
            }

            GUI.Label(
                new Rect(Screen.width / 2 - 100, Screen.height - 200, 250, 100),
                "Press 'E' to build a fire",
                style
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[FireTrigger] Trigger entered by: {other.gameObject.name} (Tag: {other.tag})");
        
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
        }
    }
}