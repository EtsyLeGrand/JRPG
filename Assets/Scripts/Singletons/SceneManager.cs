using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    private AsyncOperation asyncScene;
    private string loadedSceneName;
    void Start()
    {
        EventManager.StartListening("FadeOutComplete", LoadPreparedScene);
    }

    private void PrepareScene(string sceneName)
    {
        loadedSceneName = sceneName;
        asyncScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncScene.allowSceneActivation = false;
    }

    public static void ChangeScene(string destination, int fightDifficulty = 1, int regionid = 1)
    {
        EventManager.TriggerEvent("FadeToBlack", new Dictionary<string, object>());
        Instance.PrepareScene(destination);
    }

    public static void ChangeScene(string destination)
    {
        EventManager.TriggerEvent("FadeToBlack", new Dictionary<string, object>());
        Instance.PrepareScene(destination);
    }

    public static bool IsSceneLoaded(string scene)
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == scene;
    }

    private void LoadPreparedScene(Dictionary<string, object> _)
    {
        asyncScene.allowSceneActivation = true;

        WaitForSceneLoaded();
    }

    private IEnumerator WaitForSceneLoaded()
    {
        while (!asyncScene.isDone)
        {
            yield return null;
        }

        EventManager.TriggerEvent("On" + loadedSceneName + "SceneLoaded", null);
        EventManager.TriggerEvent("UndoFade", null);

        asyncScene = null;
        loadedSceneName = null;
    }
}
