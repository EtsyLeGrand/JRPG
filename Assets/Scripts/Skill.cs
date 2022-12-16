using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "JRPG/Skill")]
public class Skill : ScriptableObject
{
    public enum Target
    {
        Enemy = 1,
        Self = 1
    }

    public enum EffectType
    {
        InstDamage,
        InstHeal
    }

    public string skillName;
    public Vector2 skillPowerRange;
    public float manaCost;
    public Target target;
    public EffectType effect;
    public GameObject fx;
}