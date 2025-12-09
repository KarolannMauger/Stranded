using System.Collections;
using UnityEngine;
using TMPro;

// source: https://www.youtube.com/watch?v=8oTYabhj248
// with some adjustments to display dialogue at different times
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        // clic to skip typing effect or go to next line
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    // shows dialogue box and start writing the lines
    public void StartDialogue(string[] newLines)
    {
        lines = newLines;
        index = 0;

        textComponent.text = string.Empty;
        gameObject.SetActive(true);

        StartCoroutine(TypeLine());
    }

    // typing effect on dialogue
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // go to next line or removes dialogue box
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // public method to check if dialogue active
    public bool IsDialogueActive()
    {
        return gameObject.activeSelf;
    }
}