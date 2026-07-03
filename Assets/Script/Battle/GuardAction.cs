using UnityEngine;

/// Aksi bertahan. Mengaktifkan flag isGuarding sehingga damage berikutnya dikurangi defense.

public class GuardAction : IBattleAction
{
    public void Execute(BaseCharacter attacker, BaseCharacter target)
    {
        BattleAnimator attackerAnim = attacker.GetComponent<BattleAnimator>();
        if (attackerAnim != null) attackerAnim.PlayGuard();

        Debug.Log($"🛡️ {attacker.stats.characterName} bersiap bertahan! Serangan berikutnya akan dikurangi oleh defense-nya.");

        attacker.isGuarding = true;
    }
}