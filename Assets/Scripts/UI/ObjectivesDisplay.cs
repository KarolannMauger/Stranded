using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectivesDisplay : MonoBehaviour
{
    [Header("Ref Points")]
    public GameObject PlayerCapsule;
    public GameObject ShackDoor;

    [Header("Text Box Display")]
    public TMP_Text textMeshPro;
    
    [Header("Dialogue")]
    public Dialogue dialogueUI;
    public string[] midDistanceDialogueLines;

    private float DistancePlayerShack = 0;
    private float initialDistance = 0;
    private bool midDistanceDialogueTriggered = false;
    private bool objectiveChanged = false;

    void Start()
    {
        // Calculer la distance initiale
        initialDistance = Vector3.Distance(PlayerCapsule.transform.position, ShackDoor.transform.position);
    }

    void Update()
    {
        DistancePlayerShack = Vector3.Distance(PlayerCapsule.transform.position, ShackDoor.transform.position);
        
        // Trigger dialogue à mi-distance (une seule fois)
        float midDistance = initialDistance / 2f;
        if (!midDistanceDialogueTriggered && DistancePlayerShack <= midDistance)
        {
            midDistanceDialogueTriggered = true;
            StartCoroutine(TriggerMidDistanceDialogue());
        }
        
        // Changer l'objectif en dessous de 5m (une seule fois)
        if (!objectiveChanged && DistancePlayerShack < 10f)
        {
            objectiveChanged = true;
            textMeshPro.text = "Objectives: \n" +
                               "Enter the shack";
        }
        else if (!objectiveChanged)
        {
            // Afficher l'objectif normal avec la distance
            textMeshPro.text = "Objectives: \n" +
                               "Find the shack (" + DistancePlayerShack.ToString("F1") + " m)";
        }
    }
    
    IEnumerator TriggerMidDistanceDialogue()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (dialogueUI != null && midDistanceDialogueLines.Length > 0)
        {
            dialogueUI.StartDialogue(midDistanceDialogueLines);
        }
    }
}