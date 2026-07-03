using UnityEngine;


/// Implementasi karakter musuh.

public class EnemyCharacter : BaseCharacter
{
    public Sprite defaultSprite;

    void Start()
    {
        stats.currentHP = stats.maxHP;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && defaultSprite != null) sr.sprite = defaultSprite;

        Animator anim = GetComponent<Animator>();
        
    }
}