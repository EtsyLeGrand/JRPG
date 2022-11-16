using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Sprite[] emptySaveSprite;
    [SerializeField] private Sprite[] defaultSaveSprite;

    private int index;
    private SaveData saveData;

    public int Index { get => index; }

    private void Start()
    {
        RefreshLook();
    }

    public void SetSave(SaveData saveData)
    {
        this.saveData = saveData;
        index = saveData?.index??0;
    }

    public SaveData GetSave()
    {
        return saveData;
    }

    public void Load()
    {
        GameManager.Instance.Load(saveData);
    }

    public void Delete()
    {
        index = default;
        saveData = default;
    }

    public void RefreshLook()
    {
        Image defaultImg = GetComponent<Image>();
        Button btn = GetComponent<Button>();
        SpriteState spriteState = btn.spriteState;

        if (saveData == null) // empty
        {
            defaultImg.sprite = emptySaveSprite[0];
            
            spriteState.pressedSprite = emptySaveSprite[1];
            btn.spriteState = spriteState;
        }
        else
        {
            defaultImg.sprite = defaultSaveSprite[0];

            spriteState.pressedSprite = defaultSaveSprite[1];
            btn.spriteState = spriteState;
        }
    }
}
