using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage most of the Audio of the game
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource bgmSource; // Background music source
    [SerializeField] AudioSource sfxSource; // Sound effects source
    [Header("--- Audio Slider ---")]
    [SerializeField] Slider bgmSlider; // Background music source
    [SerializeField] Slider sfxSlider; // Sound effects source

    [Header("--- BGM Clips ---")]
    [SerializeField] AudioClip[] bgmClips;// Background music clips
    [Header("--- SFX Clips ---")]
    [SerializeField] AudioClip clearSound;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip uiClickSound;
    [SerializeField] AudioClip starBlockMadeSound;


    Grid grid;

    /// <summary>
    /// Get components
    /// </summary>
    void Awake()
    {
        grid = FindObjectOfType<Grid>();
        bgmSlider.onValueChanged.AddListener(delegate { ChangeBGMVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
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
        grid.BlocksMatcher.NewStarBlockMade += PlayStarBlockSound;
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
        grid.BlocksMatcher.NewStarBlockMade -= PlayStarBlockSound;

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
    /// Play when a star block is made
    /// </summary>
    void PlayStarBlockSound()
    {
        PlaySFX(starBlockMadeSound);
    }
    /// <summary>
    /// Play a clip
    /// </summary>
    /// <param name="clip">Audio Clip</param>
    void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    /// <summary>
    /// Change SFX volume
    /// </summary>
    public void ChangeSFXVolume()
    {
        ChangeVolume(sfxSource, sfxSlider);
    }

    /// <summary>
    /// Change BGM volume
    /// </summary>
    public void ChangeBGMVolume()
    {
        ChangeVolume(bgmSource, bgmSlider);
    }

    /// <summary>
    /// Change volume of an audio source based on a slider's value
    /// </summary>
    /// <param name="source">Audio Source</param>
    /// <param name="slider">Slider</param>
    private void ChangeVolume(AudioSource source, Slider slider)
    {
        source.volume = slider.value;
    }
}
