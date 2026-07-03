using UnityEngine;


/// Implementasi karakter pemain. Memuat stats dari GameSessionManager.
/// 
public class PlayerCharacter : BaseCharacter
{
    public Sprite defaultSprite;

    void Awake()
    {
        if (GameSessionManager.Instance != null && GameSessionManager.Instance.playerStats != null)
        {
            stats = GameSessionManager.Instance.playerStats;
        }
    }

    void Start()
    {
        stats.currentHP = stats.maxHP;
        stats.currentMP = stats.maxMP;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && defaultSprite != null) sr.sprite = defaultSprite;

        Animator anim = GetComponent<Animator>();
       
    }
}