using UnityEngine;
using System.Collections;

public class BeginningTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public Dialogue dialogueUI;
    public string[] linesToSay;

    [Header("UI Controller")]
    public UIController uiController;

    void Start()
    {
        StartCoroutine(DelayedStart());
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
    }
}