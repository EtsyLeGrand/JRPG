using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
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
    }

    public static void PrepareScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    public static void ChangeScene(string destination, int difficulty, int regionid)
    {
        //Coroutine transition
        UnityEngine.SceneManagement.SceneManager.LoadScene(destination);
    }
}
