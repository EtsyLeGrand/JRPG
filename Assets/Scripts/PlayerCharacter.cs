using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "JRPG/PlayerCharacter")]
[System.Serializable]
public class PlayerCharacter : BaseCharacter
{
    public AnimationCurve xpNeededByLevel;

    public float getXPNeededByLevel(int level)
    {
        return xpNeededByLevel.Evaluate(level);
    }
}
