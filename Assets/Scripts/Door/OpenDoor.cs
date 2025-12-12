using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    // Controls door rotation and audio feedback when the player interacts
    [Header("Door Settings")]
    [SerializeField] private float smooth = 4.0f;
    [SerializeField] private float doorOpenAngle = 90.0f;
    [SerializeField] private GameObject door;

    [Header("Audio")]
    [SerializeField] private AudioClip closeDoorAudio;
    [SerializeField] private AudioClip openDoorAudio;

    [Header("Input System")]
    [Tooltip("Assign the Input Action Reference for 'Interact'")]
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
        // Cache rotation targets and the door's audio source if available
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + doorOpenAngle, defaultRot.z);

        if (door != null)
        {
            audioSource = door.GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        // Subscribe to the input action when this component is active
        if (interactAction != null)
        {
            interactAction.action.performed += OnInteractPerformed;
            interactAction.action.Enable();
        }
        else
        {
            //Debug.LogWarning("[OpenDoor] None InputActionReference assigned for Interact.");
        }
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid duplicate bindings or leaks
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteractPerformed;
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        // Only toggle the door when the player is inside the trigger
        if (!enter) return;

        open = !open;

        if (audioSource != null)
        {
            if (open)
            {
                // Slightly slower opening motion
                smooth = 4.0f;
                audioSource.clip = openDoorAudio;
            }
            else
            {
                // Faster closing motion
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
            //Debug.Log("Player entered the trigger area.");
            // Allow interaction once the player is in range
            enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable interaction prompt when leaving range
            enter = false;
        }
    }
}
