using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    private List<EnemyCharacter> enemies;
    private int speedCount = 0;

    public override void Awake()
    {
       
    }

    public void SetFightData(List<EnemyCharacter> enemies)
    {
        this.enemies = enemies;

    }

    private void Update()
    {
        
    }
}
