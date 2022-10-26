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
        // TEMP
        print(SaveManager.HasSaves());
        if (!SaveManager.HasSaves())
        {
            OnLoadButtonClicked();
        }
        else
        {
            GameManager.Instance.LoadGame(
                SaveManager.Load(PlayerPrefs.GetInt("LastPlayedSlot"))
                );
        }
    }

    public void OnLoadButtonClicked()
    {
        playButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);

        slotRegion.SetActive(true);
    }

    public void OnLoadButtonClicked(Dictionary<string, object> _)
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

    public void OnBackButtonClicked()
    {
        playButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);

        slotRegion.SetActive(false);
    }

    
}
