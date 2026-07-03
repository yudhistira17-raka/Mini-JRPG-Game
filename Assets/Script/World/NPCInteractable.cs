using UnityEngine;


/// Menangani interaksi dengan NPC, termasuk dialog dan pemicu cutscene duel.

public class NPCInteractable : MonoBehaviour
{
    [Header("NPC Data")]
    public string npcName = "NPC";
    [TextArea(1, 3)]
    public string[] dialogueSentences;

    [Header("Duel Conversation Data")]
    public DialogueSequence duelDialogueSequence;

    [Header("Dialog Manager Reference")]
    [SerializeField] public SimpleDialogueManager dialogueManager;

    [Header("Event System")]
    public bool isQuestGiver = false;
    public Transform questTargetPoint;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }

    private void TriggerDialogue()
    {
        if (dialogueManager != null)
        {
            if (duelDialogueSequence != null && duelDialogueSequence.lines.Count > 0)
            {
                dialogueManager.ShowDialogue(duelDialogueSequence);
            }
            else if (dialogueSentences.Length > 0)
            {
                bool showChoices = isQuestGiver;
                dialogueManager.OnYesSelected = OnYesSelected;
                dialogueManager.OnNoSelected = OnNoSelected;
                dialogueManager.ShowDialogue(npcName, dialogueSentences[0], showChoices);
            }
            else
            {
                Debug.LogWarning($"NPC {npcName} tidak memiliki data dialog!");
            }
        }
        else
        {
            Debug.LogError($"SimpleDialogueManager belum diseret ke Inspector NPC {npcName}!");
        }
    }

    private void OnYesSelected()
    {
        if (isQuestGiver && questTargetPoint != null)
        {
            CutsceneController cutscene = FindObjectOfType<CutsceneController>();
            if (cutscene != null)
            {
                cutscene.StartDuelCutscene(questTargetPoint.position, this);
            }
        }
        else
        {
            Debug.Log("Tombol Yes ditekan, tapi NPC ini bukan quest giver.");
        }
    }

    private void OnNoSelected()
    {
        Debug.Log("Tombol No ditekan.");
    }
}