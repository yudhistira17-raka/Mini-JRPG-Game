
/// Kontrak untuk semua aksi pertempuran (Attack, Skill, Guard).

public interface IBattleAction
{
    void Execute(BaseCharacter attacker, BaseCharacter target);
}