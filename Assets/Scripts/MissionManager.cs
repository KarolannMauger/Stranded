using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [Header("References")]
    public ObjectivesDisplay objectivesDisplay;
    public Dialogue dialogueUI;

    [Header("Mission 1 - Gather Resources")]
    public int requiredWood = 3;
    public int requiredRocks = 10;

    [Header("Mission 2 - Build Fire")]
    public GameObject fireLocation; // Position du feu
    public GameObject playerCapsule;
    public string[] fireDialogueLines;

    [Header("Mission 3 - Find Shack")]
    public GameObject shackDoor;
    public string[] midDistanceDialogueLines;

    // États des missions
    public enum MissionState
    {
        GatherResources,
        BuildFire,
        FindShack,
        Completed
    }

    public MissionState currentMission = MissionState.GatherResources;
    
    private float initialShackDistance;
    private bool midDistanceDialogueTriggered = false;
    private bool objectiveChanged = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (shackDoor != null && playerCapsule != null)
        {
            initialShackDistance = Vector3.Distance(playerCapsule.transform.position, shackDoor.transform.position);
        }

        UpdateObjectiveDisplay();
    }

    private void Update()
    {
        switch (currentMission)
        {
            case MissionState.GatherResources:
                CheckResourcesGathered();
                break;

            case MissionState.BuildFire:
                CheckFireDistance();
                break;

            case MissionState.FindShack:
                CheckShackDistance();
                break;
        }
    }

    private void CheckResourcesGathered()
    {
        if (GameInventory.Instance != null)
        {
            int wood = GameInventory.Instance.woodCount;
            int rocks = GameInventory.Instance.rockCount;

            objectivesDisplay.UpdateObjective(
                $"Gather resources:\n" +
                $"Wood: {wood}/{requiredWood}\n" +
                $"Rocks: {rocks}/{requiredRocks}"
            );

            // Vérifier si les ressources sont complètes
            if (wood >= requiredWood && rocks >= requiredRocks)
            {
                AdvanceToFireMission();
            }
        }
    }

    private void AdvanceToFireMission()
    {
        currentMission = MissionState.BuildFire;
        UpdateObjectiveDisplay();
    }

    private void CheckFireDistance()
    {
        if (playerCapsule == null || fireLocation == null) return;

        float distance = Vector3.Distance(playerCapsule.transform.position, fireLocation.transform.position);
        objectivesDisplay.UpdateObjective($"Find a place to build a fire\n({distance:F1} m to the beach)");
    }

    // Appelé par FireTrigger quand le joueur active le feu
    public void OnFireBuilt()
    {
        StartCoroutine(FireBuiltSequence());
    }

    private IEnumerator FireBuiltSequence()
    {
        yield return new WaitForSeconds(0.1f);

        if (dialogueUI != null && fireDialogueLines.Length > 0)
        {
            dialogueUI.StartDialogue(fireDialogueLines);

            while (dialogueUI.IsDialogueActive())
            {
                yield return null;
            }
        }

        // Passer à la mission suivante
        currentMission = MissionState.FindShack;
        UpdateObjectiveDisplay();
    }

    private void CheckShackDistance()
    {
        if (playerCapsule == null || shackDoor == null) return;

        float distance = Vector3.Distance(playerCapsule.transform.position, shackDoor.transform.position);

        // Dialogue à mi-distance
        float midDistance = initialShackDistance / 2f;
        if (!midDistanceDialogueTriggered && distance <= midDistance)
        {
            midDistanceDialogueTriggered = true;
            StartCoroutine(TriggerMidDistanceDialogue());
        }

        // Changer objectif près de la cabane
        if (!objectiveChanged && distance < 10f)
        {
            objectiveChanged = true;
            objectivesDisplay.UpdateObjective("Enter the shack");
        }
        else if (!objectiveChanged)
        {
            objectivesDisplay.UpdateObjective($"Find the shack ({distance:F1} m)");
        }
    }

    private IEnumerator TriggerMidDistanceDialogue()
    {
        yield return new WaitForSeconds(0.1f);

        if (dialogueUI != null && midDistanceDialogueLines.Length > 0)
        {
            dialogueUI.StartDialogue(midDistanceDialogueLines);
        }
    }

    private void UpdateObjectiveDisplay()
    {
        switch (currentMission)
        {
            case MissionState.GatherResources:
                // Sera mis à jour dans CheckResourcesGathered()
                break;

            case MissionState.BuildFire:
                // Sera mis à jour dans CheckFireDistance()
                CheckFireDistance();
                break;

            case MissionState.FindShack:
                // Sera mis à jour dans CheckShackDistance()
                break;
        }
    }
}