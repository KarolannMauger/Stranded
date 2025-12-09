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
    public GameObject fireLocation;
    public GameObject playerCapsule;
    public string[] fireDialogueLines;

    [Header("Mission 3 - Find Shack")]
    public GameObject shackDoor;
    public string[] midDistanceDialogueLines;

    // mission states
    public enum MissionState
    {
        GatherResources,
        BuildFire,
        FindShack,
        Completed
    }

    // initial mission : find ressources
    public MissionState currentMission = MissionState.GatherResources;
    
    private float shackMissionStartDistance;
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

            // If ressources are complete -> prompt go to fire
            if (wood >= requiredWood && rocks >= requiredRocks)
            {
                AdvanceToFireMission();
            }
        }
    }

    // prompt go to fire 
    private void AdvanceToFireMission()
    {
        currentMission = MissionState.BuildFire;
        UpdateObjectiveDisplay();
    }

    // update distance between player and fire on screen
    private void CheckFireDistance()
    {
        if (playerCapsule == null || fireLocation == null) return;

        float distance = Vector3.Distance(playerCapsule.transform.position, fireLocation.transform.position);
        objectivesDisplay.UpdateObjective($"Find a place to build a fire\n({distance:F1} m to the beach)");
    }

    // when fire is "built" prompts dialogue
    public void OnFireBuilt()
    {
        StartCoroutine(FireBuiltSequence());
    }

    // display fire complete mission dialogue
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

        // next mission
        currentMission = MissionState.FindShack;
        
        // calculate halway distance from fire point
        if (shackDoor != null && playerCapsule != null)
        {
            shackMissionStartDistance = Vector3.Distance(playerCapsule.transform.position, shackDoor.transform.position);
        }
        
        UpdateObjectiveDisplay();
    }

    // update distance between player and shack on screen
    private void CheckShackDistance()
    {
        if (playerCapsule == null || shackDoor == null) return;

        float distance = Vector3.Distance(playerCapsule.transform.position, shackDoor.transform.position);

        // halfway on the road, player get halfway dialogue
        float midDistance = shackMissionStartDistance / 2f;
        if (!midDistanceDialogueTriggered && distance <= midDistance)
        {
            midDistanceDialogueTriggered = true;
            StartCoroutine(TriggerMidDistanceDialogue());
        }

        // when near shack, prompts player to enter and find radio
        if (!objectiveChanged && distance < 10f)
        {
            objectiveChanged = true;
            currentMission = MissionState.Completed;
            objectivesDisplay.UpdateObjective("Enter the shack and find the radio");
        }
        else if (!objectiveChanged)
        {
            objectivesDisplay.UpdateObjective($"Find the shack ({distance:F1} m)");
        }
    }

    // triggers dialogue
    private IEnumerator TriggerMidDistanceDialogue()
    {
        yield return new WaitForSeconds(0.1f);

        if (dialogueUI != null && midDistanceDialogueLines.Length > 0)
        {
            dialogueUI.StartDialogue(midDistanceDialogueLines);
        }
    }

    // display the current objective
    private void UpdateObjectiveDisplay()
    {
        switch (currentMission)
        {
            case MissionState.GatherResources:
                break;

            case MissionState.BuildFire:
                CheckFireDistance();
                break;

            case MissionState.FindShack:
                break;
        }
    }
}