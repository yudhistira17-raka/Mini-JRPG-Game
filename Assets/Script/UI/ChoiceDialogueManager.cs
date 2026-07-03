using UnityEngine;
using UnityEngine.UI;


/// Manajer dialog pilihan Yes/No (opsional, mungkin bisa digabung dengan SimpleDialogueManager).

public class ChoiceDialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    public Button yesButton;
    public Button noButton;

    private System.Action onYesSelected;

    void Start()
    {
        dialoguePanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    public void ShowChoiceDialogue(string name, string question, System.Action yesAction)
    {
        nameText.text = name;
        dialogueText.text = question;

        onYesSelected = yesAction;

        dialoguePanel.SetActive(true);
        Canvas.ForceUpdateCanvases();

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void OnYesClicked()
    {
        dialoguePanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        onYesSelected?.Invoke();
    }

    private void OnNoClicked()
    {
        dialoguePanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}