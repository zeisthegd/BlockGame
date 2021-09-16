using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage most of the Audio of the game
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource bgmSource; // Background music source
    [SerializeField] AudioSource sfxSource; // Sound effects source

    [Header("--- BGM Clips ---")]
    [SerializeField] AudioClip[] bgmClips;// Background music clips
    [Header("--- SFX Clips ---")]
    [SerializeField] AudioClip clearSound;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip uiClickSound;

    Grid grid;

    /// <summary>
    /// Get components
    /// </summary>
    void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    /// <summary>
    /// Check if current BGM has ended and play new BGM
    /// </summary>
    void Update()
    {
        if (!bgmSource.isPlaying)
            PlayRandomBGM();
    }

    /// <summary>
    /// Connect events
    /// Play random BGM
    /// </summary>
    void Start()
    {
        grid.BlocksMatcher.BeginClearingBlocks += PlayClearSound;
        grid.BlocksMatcher.BeginSwappingBlocks += PlayMoveSound;
        DontDestroyOnLoad(this);
        PlayRandomBGM();
    }

    /// <summary>
    /// Disconnect events
    /// </summary>
    void OnDisable()
    {
        grid.BlocksMatcher.BeginClearingBlocks -= PlayClearSound;
        grid.BlocksMatcher.BeginSwappingBlocks -= PlayMoveSound;
    }

    /// <summary>
    /// Choose a random Audio clip and play it
    /// </summary>
    void PlayRandomBGM()
    {
        bgmSource.clip = bgmClips[Random.Range(0, bgmClips.Length)];
        bgmSource.Play();
    }

    /// <summary>
    /// Play when the matches are cleared.
    /// </summary>
    void PlayClearSound()
    {
        PlaySFX(clearSound);
    }

    /// <summary>
    /// Play when 2 blocks are swapped
    /// </summary>
    void PlayMoveSound()
    {
        PlaySFX(moveSound);
    }

    /// <summary>
    /// Play when a button is clicked
    /// </summary>
    public void PlayUIClickSound()
    {
        PlaySFX(uiClickSound);
    }

    /// <summary>
    /// Play a clip
    /// </summary>
    /// <param name="clip">Audio Clip</param>
    void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
