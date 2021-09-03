using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [Header("--- Child UIs ---")]
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] Grid grid;
    public bool gameIsPaused;
    void Awake()
    {
        grid.NotEnoughBlocksToPlay += GameOver;
    }
    void Start()
    {
        Resume();
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseMenuUI.transform.Find("ResumeButton").gameObject.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void GameOver()
    {
        pauseMenuUI.SetActive(true);
        pauseMenuUI.transform.Find("ResumeButton").gameObject.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
