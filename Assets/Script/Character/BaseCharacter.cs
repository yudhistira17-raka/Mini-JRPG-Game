using UnityEngine;


/// Kelas abstrak untuk entitas yang dapat menerima damage dan memiliki stats.

public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterStats stats;
    public bool IsDead => stats.currentHP <= 0;
    public bool isGuarding = false;

    public virtual void TakeDamage(int rawDamage)
    {
        if (IsDead) return;

        int finalDamage;
        if (isGuarding)
        {
            finalDamage = Mathf.Max(1, rawDamage - stats.defense);
            isGuarding = false;
        }
        else
        {
            finalDamage = rawDamage;
        }

        stats.currentHP -= finalDamage;
        Debug.Log($"{stats.characterName} menerima {finalDamage} damage! Sisa HP: {stats.currentHP}");

        // Perbarui UI jika ada
        CharacterUI ui = GetComponent<CharacterUI>();
        if (ui != null)
        {
            ui.UpdateHP(stats.currentHP, stats.maxHP);
            ui.UpdateMP(stats.currentMP, stats.maxMP);
        }

        if (stats.currentHP <= 0)
        {
            stats.currentHP = 0;
            Debug.Log($"{stats.characterName} telah K.O!");
            BattleAnimator anim = GetComponent<BattleAnimator>();
            if (anim != null) anim.PlayDeath();
        }
    }

    public virtual void Heal(int amount)
    {
        stats.currentHP = Mathf.Min(stats.maxHP, stats.currentHP + amount);
        CharacterUI ui = GetComponent<CharacterUI>();
        if (ui != null) ui.UpdateHP(stats.currentHP, stats.maxHP);
    }
}