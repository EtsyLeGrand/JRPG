using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "JRPG/EnemyCharacter")]
[System.Serializable]
public class EnemyCharacter : BaseCharacter
{
    public Vector2 levelRange = new Vector2(1, 1);
    public int speed;
    public AnimationCurve xpLoot;

    public float GetXP(int level)
    {
        return xpLoot.Evaluate(level);
    }
}
