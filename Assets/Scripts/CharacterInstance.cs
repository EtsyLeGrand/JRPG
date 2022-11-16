using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance
{
    [System.Serializable]
    public struct CurrentStats
    {
        public int level;
        public int hp;

        public float strength;
        public float constitution;
        public float mana;
        public float initiative;
        public float luck;
    }

    public BaseCharacter character;
    public CurrentStats currentStats = new CurrentStats();
    public List<RPGClass.SkillUnlock> Skills => character.rpgClass.SkillUnlocks;

    public CharacterInstance(BaseCharacter character)
    {
        this.character = character;
        currentStats = new CurrentStats()
        {
            level = 1,

            strength = character.rpgClass.GetStrength(1),
            mana = character.rpgClass.GetMana(1),
            constitution = character.rpgClass.GetConstitution(1),
            initiative = character.rpgClass.GetInitiative(1),
            luck = character.rpgClass.GetLuck(1),
        };

        currentStats.hp = (int)currentStats.constitution; // À CHANGER
    }

    public void Heal(int hp)
    {
        currentStats.hp += hp;
    }

    public void Damage(int hp)
    {
        currentStats.hp -= hp;
    }
}
