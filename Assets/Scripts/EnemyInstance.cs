using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    private RPGClass.Stats stats;
    private int speed;

    public void Init(EnemyCharacter enemy, int speed)
    {
        stats = enemy.rpgClass.stats;
        this.speed = speed;
    }

}
