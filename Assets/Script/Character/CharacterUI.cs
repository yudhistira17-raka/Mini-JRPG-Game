using UnityEngine;
using UnityEngine.UI;


/// Menampilkan dan memperbarui teks HP dan MP pada UI.

public class CharacterUI : MonoBehaviour
{
    public Text hpText;
    public Text mpText;

    public void UpdateHP(int currentHP, int maxHP)
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHP} / {maxHP}";
        }
    }

    public void UpdateMP(int currentMP, int maxMP)
    {
        if (mpText != null)
        {
            mpText.text = $"MP: {currentMP} / {maxMP}";
        }
    }
}