using UnityEngine;
using TMPro;

public class ObjectivesDisplay : MonoBehaviour
{
    [Header("Text Box Display")]
    public TMP_Text textMeshPro;

    public void UpdateObjective(string objectiveText)
    {
        textMeshPro.text = "Objectives:\n" + objectiveText;
    }
}