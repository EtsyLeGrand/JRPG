using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightActor : MonoBehaviour
{
    public FightInfoCanvas fightInfoCanvas;
    public int initiative;
    public int fighterIndex;


    private void OnMouseOver()
    {
        fightInfoCanvas.SetDisplayInfo(fighterIndex);
    }
}
