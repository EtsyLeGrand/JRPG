using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Sprite[] emptySaveSprite;
    [SerializeField] private Sprite[] defaultSaveSprite;

    private int index;
    private SaveData saveData;

    private void Start()
    {
        Image thisImg = GetComponent<Image>();
        Button thisBtn = GetComponent<Button>();

        SpriteState tempState = thisBtn.spriteState;
        thisBtn.transition = Selectable.Transition.SpriteSwap;

        if (saveData != null)
        {
            thisImg.sprite = defaultSaveSprite[0];
            tempState.pressedSprite = defaultSaveSprite[1];
            thisBtn.spriteState = tempState;
        }
        else
        {
            thisImg.sprite = emptySaveSprite[0];
            tempState.pressedSprite = emptySaveSprite[1];
            thisBtn.spriteState = tempState;
        }
    }

    public void SetSave(SaveData saveData)
    {
        this.saveData = saveData;
        index = saveData.index;
        
        deleteButton.interactable = true;
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(DeleteSave);
        
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(LoadGame);
    }

    public void SetEmpty(int slotIndex)
    {
        index = slotIndex;

        deleteButton.interactable = false;
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(DeleteSave);
        
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(CreateNewGame);
    }

    public void DeleteSave()
    {
        SaveManager.DeleteSave(index);
    }

    public void LoadGame()
    {
        if (HasData())
        {
            GameManager.Instance.LoadGame(saveData);
        }
    }

    public bool HasData()
    {
        return saveData != null;
    }

    public void CreateNewGame()
    {
        GameManager.Instance.NewGame(index);
    }
}
