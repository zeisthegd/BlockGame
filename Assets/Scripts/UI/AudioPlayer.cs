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
}
