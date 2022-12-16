using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightInfoCanvas : MonoBehaviour
{
    [SerializeField] private Text hoveredHpCount;
    [SerializeField] private Text hoveredManaCount;
    [SerializeField] private Text hoveredActorName;
    [SerializeField] public GameObject winSection;
    [SerializeField] public GameObject loseSection;
    [SerializeField] public GameObject bossBeatenSection;

    public void SetDisplayInfo(int index)
    {
        CharacterInstance inst = FightManager.Instance.GetMemberByIndex(index);
        hoveredHpCount.text = "HP: " + inst.currentStats.hp;
        hoveredManaCount.text = "Mana: " + inst.currentStats.mana;
        hoveredActorName.text = inst.character.rpgClass.className;
    }

    public void OnBackToMenuClicked()
    {
        FightManager.Instance.BackToMenu();
    }

    public void OnBackToMapClicked()
    {
        FightManager.Instance.BackToMap();
    }
}
