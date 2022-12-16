using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private bool isAnimationOver = false;
    private bool isAudioOver = false;
    private float cnt = 0;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (cnt >= source.clip.length)
        {
            isAudioOver = true;
        }

        if (isAudioOver && isAnimationOver)
        {
            Destroy(gameObject);
        }
        cnt += Time.deltaTime;
    }
    public void OnFXOver()
    {
        isAnimationOver = true;
        Destroy(gameObject);
    }
}
