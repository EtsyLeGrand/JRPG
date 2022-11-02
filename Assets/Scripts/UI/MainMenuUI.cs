using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject slotRegion;
    [SerializeField] private Button backButton;

    private void Start()
    {
        EventManager.StartListening("SpawnLoadMenu", OnLoadButtonClicked);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("SpawnLoadMenu", OnLoadButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        if (!PlayerPrefs.HasKey("LastSlotPlayed"))
        {
            SaveManager.Instance.SetNewSaveAtSlot(0);
            SaveManager.Instance.LoadSlot(0);
        }
        else
        {
            SaveManager.Instance.LoadSlot(PlayerPrefs.GetInt("LastSlotPlayed"));
        }
    }
    public void OnLoadButtonClicked(Dictionary<string, object> _)
    {
        OnLoadButtonClicked();
    }
    public void OnLoadButtonClicked()
    {
        playButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);

        slotRegion.SetActive(true);

        RefreshSlotsSprites();
    }

    public void OnSlotClicked(int index)
    {
        if (SaveManager.Instance.IsSlotEmpty(index))
        {
            SaveManager.Instance.SetNewSaveAtSlot(index);
            RefreshSlotsSprites();
        }
        else
        {
            SaveManager.Instance.LoadSlot(index);
        }
    }

    public void OnDeleteSlotClicked(int index)
    {
        if (!SaveManager.Instance.IsSlotEmpty(index))
        {
            SaveManager.Instance.DeleteSlot(index);
        }
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnBackButtonClicked()
    {
        playButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);

        slotRegion.SetActive(false);
    }

    private void RefreshSlotsSprites()
    {
        SaveManager.Instance.RefreshAll();
    }
}
