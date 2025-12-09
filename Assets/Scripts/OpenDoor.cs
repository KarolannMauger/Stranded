using UnityEngine;
using UnityEngine.InputSystem;   // Nouveau Input System
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float smooth = 4.0f;
    [SerializeField] private float doorOpenAngle = 90.0f;
    [SerializeField] private GameObject door;

    [Header("Audio")]
    [SerializeField] private AudioClip closeDoorAudio;
    [SerializeField] private AudioClip openDoorAudio;

    [Header("Input System")]
    [Tooltip("Réfère à l'action 'Interact' dans ton Input Actions Asset")]
    public InputActionReference interactAction;

    [Header("UI Style")]
    public Font customFont;
    public int fontSize = 30;

    private bool open;
    private bool enter;
    private Vector3 defaultRot;
    private Vector3 openRot;
    private AudioSource audioSource;

    private void Start()
    {
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + doorOpenAngle, defaultRot.z);

        if (door != null)
        {
            audioSource = door.GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteractPerformed;
            interactAction.action.Enable();
        }
        else
        {
            Debug.LogWarning("[OpenDoor] Aucun InputActionReference assigné pour Interact.");
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

    private void Update()
    {
        // Rotation smooth de la porte
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
        }
    }

    // Appelée quand l'action "Interact" est déclenchée (E, bouton manette, etc.)
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!enter) return; // On ne peut ouvrir que si le joueur est dans la zone

        open = !open;

        if (audioSource != null)
        {
            if (open)
            {
                smooth = 4.0f;
                audioSource.clip = openDoorAudio;
            }
            else
            {
                smooth = 10.0f;
                audioSource.clip = closeDoorAudio;
            }
            audioSource.Play();
        }
    }

    private void OnGUI()
    {
        if (enter)
        {
            // personnalized style
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = fontSize;
            style.alignment = TextAnchor.MiddleCenter;

            // custom font if assigned
            if (customFont != null)
            {
                style.font = customFont;
            }

            GUI.Label(
                new Rect(Screen.width / 2 - 100, Screen.height - 200, 250, 100),
                "Press 'E' to open the door",
                style
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger area.");
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
