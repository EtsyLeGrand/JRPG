using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "JRPG/Skill")]
public class Skill : ScriptableObject
{
    [Flags]
    public enum Target
    {
        Enemy = 1 << 0,
        HeroParty = 1 << 1,
        Self = 1 << 2
    }

    [System.Serializable]
    public class TickSetting
    {
        public Effect.TickType tickType;
        [Range(1, 100)] public int probability;
        public int turnLength;
        public Vector2 tickValueRange;
    }

    [System.Serializable]
    public struct Effect
    {
        [Flags]
        public enum EffectType
        {
            InstDamage = 1 << 0,
            InstHeal = 1 << 1,
            TickDamage = 1 << 2,
            TickHealing = 1 << 3
        }

        [Flags]
        public enum TickType
        {
            Heal,
            Burn,
            Poison,
            Stun
        }

        public EffectType type;
        public TickSetting setting;
    }

    [Flags]
    public enum SkillType
    {
        Normal = 1 << 0,
        Fire = 1 << 1,
        Poison = 1 << 2,
    }

    public SkillType skillType;
    public Vector2 skillPowerRange;
    public float manaCost;
    public Target target;
    public Effect effect;
    public GameObject vfx;
    public GameObject tickVfx;
    public GameObject sfx;
    public GameObject tickSfx;
}
