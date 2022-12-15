using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RPGClass", menuName = "JRPG/Class")]
public class RPGClass : ScriptableObject
{
    [System.Serializable]
    public struct Stats
    {
        public AnimationCurve strength;
        public AnimationCurve constitution;
        public AnimationCurve mana;
        public AnimationCurve initiative;
        public AnimationCurve luck;
    }

    [System.Serializable]
    public struct SkillUnlock
    {
        public int level;
        public Skill skill;
    }

    public string className;
    public Stats stats;
    public List<SkillUnlock> SkillUnlocks;

    public float GetStrength(int level)
    {
        return stats.strength.Evaluate(level);
    }
    public float GetMana(int level)
    {
        return stats.mana.Evaluate(level);
    }
    public float GetConstitution(int level)
    {
        return stats.constitution.Evaluate(level);
    }
    public float GetInitiative(int level)
    {
        return stats.initiative.Evaluate(level);
    }
    public float GetLuck(int level)
    {
        return stats.luck.Evaluate(level);
    }

}
