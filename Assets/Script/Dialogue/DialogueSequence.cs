using UnityEngine;
using System.Collections.Generic;


/// Struktur satu baris dialog.

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)] public string sentence;
}

/// ScriptableObject yang menyimpan urutan dialog (percakapan).

[CreateAssetMenu(fileName = "New Dialogue", menuName = "JRPG/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}


