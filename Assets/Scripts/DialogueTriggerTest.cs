using UnityEngine;
using System.Collections;

public class DialogueTriggerTest : MonoBehaviour
{
    public Dialogue dialogueUI;
    public string[] linesToSay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);

        dialogueUI.StartDialogue(linesToSay);
    }
}
