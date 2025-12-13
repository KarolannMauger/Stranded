using UnityEngine;
using System.Collections;

public class BeginningTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public Dialogue dialogueUI;
    public string[] linesToSay;

    [Header("UI Controller")]
    public UIController uiController;

    private static bool hasTriggered = false;

    void Start()
    {
        // do not trigger beginning sequence if player respawn
        if (!hasTriggered)
        {
            StartCoroutine(DelayedStart());
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f);

        if (dialogueUI != null && linesToSay.Length > 0)
        {
            dialogueUI.StartDialogue(linesToSay);

            // Open book after dialogue
            while (dialogueUI.IsDialogueActive())
            {
                yield return null;
            }

            if (uiController != null)
            {
                uiController.ToggleHelp();
            }
        }

        hasTriggered = true;
    }

    // to reset trigger when called from main menu
    public static void ResetTrigger()
    {
        hasTriggered = false;
    }
}