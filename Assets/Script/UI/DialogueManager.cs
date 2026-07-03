using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// Manajer dialog dengan antrean kalimat (digunakan untuk dialog sederhana).

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string name, string[] newSentences)
    {
        dialoguePanel.SetActive(true);
        nameText.text = name;

        sentences.Clear();
        foreach (string sentence in newSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("Dialog selesai.");
    }
}