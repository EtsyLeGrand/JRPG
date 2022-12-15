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

        public int strength;
        public int constitution;
        public int mana;
        public int initiative;
        public int luck;
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

            strength = (int)character.rpgClass.GetStrength(1),
            mana = (int)character.rpgClass.GetMana(1),
            constitution = (int)character.rpgClass.GetConstitution(1),
            initiative = (int)character.rpgClass.GetInitiative(1),
            luck = (int)character.rpgClass.GetLuck(1),
        };

        currentStats.hp = currentStats.constitution; // À CHANGER
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
