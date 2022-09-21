using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseCharacter : ScriptableObject
{
    public RPGClass rpgClass;
    public RuntimeAnimatorController animatorController;
    public Sprite sprite;
}
