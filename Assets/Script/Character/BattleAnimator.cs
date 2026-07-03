using UnityEngine;


/// Memicu animasi pertempuran (Attack, Skill, Guard, Death) pada Animator.

public class BattleAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator tidak ditemukan pada karakter.");
    }

    public void PlayAttack()
    {
        if (animator != null) animator.SetTrigger("TriggerAttack");
    }

    public void PlaySkill()
    {
        if (animator != null) animator.SetTrigger("TriggerSkill");
    }

    public void PlayGuard()
    {
        if (animator != null) animator.SetTrigger("TriggerGuard");
    }

    public void PlayDeath()
    {
        if (animator != null) animator.SetTrigger("TriggerLose");
    }
}