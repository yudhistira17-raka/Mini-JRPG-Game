using UnityEngine;


/// Aksi serangan fisik biasa. Damage random 1~ATK, dengan kemungkinan Critical.

public class AttackAction : IBattleAction
{
    public void Execute(BaseCharacter attacker, BaseCharacter target)
    {
        BattleAnimator attackerAnim = attacker.GetComponent<BattleAnimator>();
        if (attackerAnim != null) attackerAnim.PlayAttack();

        int atk = Mathf.Max(1, attacker.stats.attack);
        int baseDamage = Random.Range(1, atk + 1);

        float finalMultiplier = 1f;
        int randomChance = Random.Range(0, 100);
        if (randomChance < attacker.stats.critRate)
        {
            finalMultiplier = attacker.stats.critDamage;
            Debug.Log($"💥 CRITICAL HIT! ({finalMultiplier * 100}% damage)");
        }

        int finalDamage = Mathf.RoundToInt(baseDamage * finalMultiplier);
        target.TakeDamage(finalDamage);
    }
}