using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveName;
    [SerializeField] private TextMeshProUGUI loadButtonText;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    private int index;
    private SaveData saveData;
    
    public void SetSave(SaveData _saveData)
    {
        saveData = _saveData;
        index = saveData.index;
        
        saveName.text = $"Save {_saveData.index+1}";
        loadButtonText.text = "Load";
        
        deleteButton.interactable = true;
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(DeleteSave);
        
        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(LoadGame);
    }

    public void SetEmpty(int _slotIndex)
    {
        index = _slotIndex;
        
        saveName.text = "Empty";
        loadButtonText.text = "New Game";

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
        //Call GameManager LoadGame(saveData)
    }

    public void CreateNewGame()
    {
        GameManager.Instance.NewGame(index);
    }
}
