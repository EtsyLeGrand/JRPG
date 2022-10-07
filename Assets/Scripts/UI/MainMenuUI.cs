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
    [SerializeField] private Button[] slotButton;
    [SerializeField] private Button[] deleteSlotButton;
    [SerializeField] private Button backButton;

    public void OnPlayButtonClicked()
    {
        /*
        if (PlayerPrefs.HasKey("LastPlayedSlot"))
        {
            OnSaveSlotClicked(PlayerPrefs.GetInt("LastPlayedSlot"));
        }
        else
        {

        }
        */
        SceneManager.ChangeScene("Map");
    }

    public void OnLoadButtonClicked()
    {
        playButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);

        slotRegion.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnSaveSlotClicked(int index)
    {
        
        //PlayerPrefs.SetInt("LastPlayedSlot", index);
    }

    public void OnDeleteSaveSlotClicked(int index)
    {

    }

    public void OnBackButtonClicked()
    {
        playButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);

        slotRegion.SetActive(false);
    }
}
