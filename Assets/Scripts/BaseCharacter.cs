using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseCharacter : ScriptableObject
{
    public RPGClass rpgClass;
    public RuntimeAnimatorController animatorController;
    public Sprite sprite;
    public Vector3 spriteScale;
    public Vector2 fightColliderOffset;
    public Vector2 fightColliderSize;
}
