using UnityEngine;


/// Aksi skill. Damage random 1~ATK × 1.5, mengurangi MP sebesar biaya yang ditentukan.

public class SkillAction : IBattleAction
{
    private string skillName;
    private int mpCost;

    public SkillAction(string name, int mpCostAmount = 5)
    {
        skillName = name;
        mpCost = mpCostAmount;
    }

    public void Execute(BaseCharacter attacker, BaseCharacter target)
    {
        if (attacker.stats.currentMP < mpCost)
        {
            Debug.Log($"⚠️ {attacker.stats.characterName} tidak cukup MP untuk {skillName}!");
            return;
        }

        attacker.stats.currentMP -= mpCost;

        BattleAnimator attackerAnim = attacker.GetComponent<BattleAnimator>();
        if (attackerAnim != null) attackerAnim.PlaySkill();

        int baseDamage = Mathf.RoundToInt(Random.Range(1, attacker.stats.attack + 1) * 1.5f);

        float finalMultiplier = 1f;
        int randomChance = Random.Range(0, 100);
        if (randomChance < attacker.stats.critRate)
        {
            finalMultiplier = attacker.stats.critDamage;
            Debug.Log($"💥 CRITICAL SKILL! ({finalMultiplier * 100}% damage)");
        }

        int finalDamage = Mathf.RoundToInt(baseDamage * finalMultiplier);
        Debug.Log($"🔥 {attacker.stats.characterName} menggunakan {skillName} dengan {finalDamage} damage!");
        target.TakeDamage(finalDamage);
    }
}