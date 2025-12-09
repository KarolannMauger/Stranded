using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public Dialogue dialogueUI;
    public string[] endGameDialogueLines;

    [Header("Input System")]
    public InputActionReference interactAction;

    [Header("UI Style")]
    public Font customFont;
    public int fontSize = 30;

    [Header("Scene Management")]
    [SerializeField] private SceneController sceneController;
    public string gameOverSceneName = "HappyEndScene";

    [Header("Audio")]
    public AudioClip radioStaticSound;
    private AudioSource audioSource;

    private bool enter;
    private bool dialogueTriggered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
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
        if (!enter || dialogueTriggered) return;

        // if mission is not null and "completed" mission
        if (MissionManager.Instance != null)
        {
            if (MissionManager.Instance.currentMission == MissionManager.MissionState.Completed)
            {
                dialogueTriggered = true;
                StartCoroutine(TriggerEndGameSequence());
            }
        }
    }

    // trigger end sequence: back and forth dialogue + radio sound -> goes to end screen
    private IEnumerator TriggerEndGameSequence()
    {
        if (radioStaticSound != null && audioSource != null)
        {
            audioSource.clip = radioStaticSound;
            audioSource.loop = true;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.1f);

        if (dialogueUI != null && endGameDialogueLines.Length > 0)
        {
            dialogueUI.StartDialogue(endGameDialogueLines);
            
            while (dialogueUI.IsDialogueActive())
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(2f);
            
            if (sceneController != null)
            {
                sceneController.LoadScene(gameOverSceneName);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
            }
        }
    }

    private void OnGUI()
    {
        // Only prompts player when it's the right mission
        if (enter && !dialogueTriggered && 
            MissionManager.Instance != null && 
            MissionManager.Instance.currentMission == MissionManager.MissionState.Completed)
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
                "Press 'E' to use radio",
                style
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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