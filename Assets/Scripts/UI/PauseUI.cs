using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadPreviousButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitToDesktopButton;

    [SerializeField] private GameObject pauseSection;

    public void TogglePauseMenu()
    {
        saveButton.gameObject.SetActive(true);
        pauseSection.SetActive(!pauseSection.activeInHierarchy);
    }

    public void ToggleSaveMenu()
    {
        saveButton.gameObject.SetActive(!saveButton.gameObject.activeInHierarchy);
    }

    public void QuitToMenu()
    {
        SceneManager.ChangeScene("MainMenu");
    }

    public void QuitToDesktop()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadSave()
    {

    }

    public void QuickSave()
    {
        

    }
}
