using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    private static FadeUI instance;

    [SerializeField] private Image blackFade;
    [SerializeField] private float fadeTime = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        EventManager.StartListening("FadeToBlack", FadeOutEvent);
        EventManager.StartListening("UndoFade", FadeInEvent);
    }

    private void FadeOutEvent(Dictionary<string, object> _)
    {
        StartCoroutine(FadeOut());
    }

    private void FadeInEvent(Dictionary<string, object> _)
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float timer = 0.0f;
        while (timer < fadeTime)
        {
            Debug.Log("Running");
            timer += Time.deltaTime;
            blackFade.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / fadeTime));
            yield return null;
        }
        EventManager.TriggerEvent("FadeOutComplete", new Dictionary<string, object>());
    }

    public IEnumerator FadeIn()
    {
        float timer = 0.0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            blackFade.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeTime));
            yield return null;
        }
    }
}
