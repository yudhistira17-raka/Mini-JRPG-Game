using UnityEngine;


/// Container data statistik karakter (HP, MP, ATK, DEF, Speed, Crit).

[System.Serializable]
public class CharacterStats
{
    public string characterName = "Hero";
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxMP = 30;
    public int currentMP = 30;
    public int attack = 15;
    public int defense = 5;
    public int speed = 10;
    [Range(0, 100)] public int critRate = 10;
    public float critDamage = 1.5f;
}