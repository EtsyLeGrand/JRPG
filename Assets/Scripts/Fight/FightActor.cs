using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightActor : MonoBehaviour
{
    public enum ActorTeam
    {
        Hero,
        Enemy
    }

    public FightInfoCanvas fightInfoCanvas;
    public int id;
    public int initiative;
    public ActorTeam team;
    

    private void OnMouseOver()
    {
        fightInfoCanvas.SetDisplayInfo(id);
    }

    private void OnMouseUp()
    {
        if (FightManager.Instance.canTarget)
        {
            FightManager.Instance.SetAttackTarget(id);
        }
    }
}
