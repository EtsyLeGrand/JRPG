using System;
using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroy = default;

    public static Action OnInitialized;
    public static Action OnStop;

    private static T instance = default;
    public static bool isClosed = false;
    public static bool isInitialized = false;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                string file = typeof(T).ToString();
                string[] split = file.Split('.');
                file = split[split.Length - 1];
                instance = Instantiate(Resources.Load<T>($"{file}"));
            }

            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            Debug.Log("Instance Exists");
        }
        else
        {
            Debug.Log("Instance Doesn't exist");
            isClosed = false;
            instance = this as T;
            if (dontDestroy)
                DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    public virtual void Init()
    {
        isInitialized = true;
        OnInitialized?.Invoke();
    }

    public virtual void Stop()
    {
        isInitialized = false;
        isClosed = true;
        OnStop?.Invoke();
    }

    protected virtual void OnDisable()
    {
        Stop();
    }

    public void ForceInit() { }
}
