using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private AsyncOperation asyncScene;
    private static SceneManager sceneManager;

    public static SceneManager instance
    {
        get
        {
            if (!sceneManager)
            {
                sceneManager = FindObjectOfType(typeof(SceneManager)) as SceneManager;

                if (!sceneManager)
                {
                    Debug.LogError("There needs to be one active SceneManager script on a GameObject in your scene.");
                }
                else
                {
                    sceneManager.Init();
                }
            }
            return sceneManager;
        }
    }

    void Init()
    {
        DontDestroyOnLoad(instance);
        EventManager.StartListening("FadeOutComplete", LoadPreparedScene);
    }

    private void PrepareScene(string sceneName)
    {
        asyncScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncScene.allowSceneActivation = false;
    }

    public static void ChangeScene(string destination, int fightDifficulty = 1, int regionid = 1)
    {
        EventManager.TriggerEvent("FadeToBlack", new Dictionary<string, object>());
        instance.PrepareScene(destination);
    }

    public static void ChangeScene(string destination)
    {
        EventManager.TriggerEvent("FadeToBlack", new Dictionary<string, object>());
        instance.PrepareScene(destination);
    }

    private void LoadPreparedScene(Dictionary<string, object> _)
    {
        asyncScene.allowSceneActivation = true;
        EventManager.TriggerEvent("UndoFade", new Dictionary<string, object>());
    }
}
