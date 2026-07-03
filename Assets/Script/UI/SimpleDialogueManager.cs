using UnityEngine;
using UnityEngine.UI;
using System;


/// Mengelola dialog teks sederhana atau dialog pilihan (Yes/No).

public class SimpleDialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    public Button yesButton;
    public Button noButton;

    public Action OnYesSelected;
    public Action OnNoSelected;
    public Action OnClosed;

    private PlayerInput playerInput;
    private bool isChoiceDialog = false;
    private DialogueSequence currentSequence;
    private int currentLineIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(false);
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isChoiceDialog)
            {
                if (yesButton != null && yesButton.gameObject.activeSelf)
                {
                    yesButton.onClick.Invoke();
                }
            }
            else if (currentSequence != null)
            {
                DisplayNextLine();
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    public void ShowDialogue(DialogueSequence sequence)
    {
        if (playerInput != null) playerInput.LockInput();

        dialoguePanel.SetActive(true);
        isChoiceDialog = false;
        currentSequence = sequence;
        currentLineIndex = 0;

        if (yesButton != null) yesButton.gameObject.SetActive(false);
        if (noButton != null) noButton.gameObject.SetActive(false);

        if (currentSequence.lines.Count > 0)
        {
            nameText.text = currentSequence.lines[currentLineIndex].speakerName;
            dialogueText.text = currentSequence.lines[currentLineIndex].sentence;
        }
    }

    private void DisplayNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < currentSequence.lines.Count)
        {
            nameText.text = currentSequence.lines[currentLineIndex].speakerName;
            dialogueText.text = currentSequence.lines[currentLineIndex].sentence;
        }
        else
        {
            CloseDialogue();
        }
    }

    public void ShowDialogue(string name, string sentence, bool showChoices = false)
    {
        if (playerInput != null) playerInput.LockInput();
        currentSequence = null;

        nameText.text = name;
        dialogueText.text = sentence;
        dialoguePanel.SetActive(true);

        isChoiceDialog = showChoices;
        if (yesButton != null) yesButton.gameObject.SetActive(showChoices);
        if (noButton != null) noButton.gameObject.SetActive(showChoices);

        if (yesButton != null) yesButton.onClick.RemoveAllListeners();
        if (noButton != null) noButton.onClick.RemoveAllListeners();

        if (yesButton != null) yesButton.onClick.AddListener(OnYesClicked);
        if (noButton != null) noButton.onClick.AddListener(OnNoClicked);
    }

    public void OnYesClicked()
    {
        var yesCallback = OnYesSelected;
        CloseDialogue();
        yesCallback?.Invoke();
    }

    public void OnNoClicked()
    {
        var noCallback = OnNoSelected;
        CloseDialogue();
        noCallback?.Invoke();
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);

        if (playerInput != null) playerInput.UnlockInput();

        if (!isChoiceDialog)
        {
            OnClosed?.Invoke();
        }

        OnYesSelected = null;
        OnNoSelected = null;
        OnClosed = null;
        currentSequence = null;
        isChoiceDialog = false;
    }
}