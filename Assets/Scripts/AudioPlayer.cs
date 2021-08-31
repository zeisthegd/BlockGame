using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] bgm;

    [SerializeField] AudioSource bgmSource;
    
    void Start()
    {
        DontDestroyOnLoad(this);
        PlayRandomBGM();
    }

    void Update()
    {
        if ((int)(bgmSource.time - bgmSource.clip.length) == 2)
            StartFade(bgmSource, 2, 0.5f);
    }

    void PlayRandomBGM()
    {
        bgmSource.clip = bgm[Random.Range(0, bgm.Length)];
        bgmSource.Play();
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        PlayRandomBGM();
        yield break;
    }
}
