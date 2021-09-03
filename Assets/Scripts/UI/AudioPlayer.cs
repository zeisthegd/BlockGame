using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--- BGM Clips ---")]
    [SerializeField] AudioClip[] bgmClips;
    [Header("--- SFX Clips ---")]
    [SerializeField] AudioClip clearSound;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip uiClickSound;

    Grid grid;

    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }
    void OnEnable()
    {
        
    }
    void Start()
    {
        grid.BlocksMatcher.BeginClearingBlocks += PlayClearSound;
        grid.BlocksMatcher.BeginSwappingBlocks += PlayMoveSound;
        DontDestroyOnLoad(this);
        PlayRandomBGM();
    }
    void Update()
    {
        if ((int)(bgmSource.time - bgmSource.clip.length) == 2)
            StartFade(bgmSource, 2, 0.5f);
    }
    void OnDisable()
    {
        grid.BlocksMatcher.BeginClearingBlocks -= PlayClearSound;
        grid.BlocksMatcher.BeginSwappingBlocks -= PlayMoveSound;
    }
    void PlayRandomBGM()
    {
        bgmSource.clip = bgmClips[Random.Range(0, bgmClips.Length)];
        bgmSource.Play();
    }

    void PlayClearSound()
    {
        PlaySFX(clearSound);
    }

    void PlayMoveSound()
    {
        PlaySFX(moveSound);
    }
    void PlayUIClickSound()
    {
        PlaySFX(uiClickSound);
    }
    void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
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
