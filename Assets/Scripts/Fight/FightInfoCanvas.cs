using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightInfoCanvas : MonoBehaviour
{
    [SerializeField] private Text hoveredHpCount;
    [SerializeField] private Text hoveredManaCount;
    [SerializeField] private Text hoveredActorName;
    


    public void SetDisplayInfo(int index)
    {
        CharacterInstance inst = FightManager.Instance.GetMemberByIndex(index);
        hoveredHpCount.text = "HP: " + inst.currentStats.hp;
        hoveredManaCount.text = "Mana: " + inst.currentStats.mana;
        hoveredActorName.text = inst.character.rpgClass.className;
    }
}
