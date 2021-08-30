using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    public bool gameIsPaused;
    void Start()
    {
        Resume();
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
