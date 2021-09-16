using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

/// <summary>
/// The root UI.
/// </summary>
public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject resumeBtn;
    [SerializeField] TMP_Text moveCountTxt; // Remaining move text.
    [SerializeField] TMP_Text finalPointsTxt; // Final result points text.
    [SerializeField] GameObject[] stars; // Star that can be obtain after total points reach a specific threshold. 3 in total.

    [SerializeField] Grid grid;

    /// <summary>
    /// Set up the UI.
    /// </summary>
    void Start()
    {
        ConnectEvents();
        Resume();
        UpdateRemainingMoves();
    }

    void OnDisable()
    {
        DisconnectEvents();
    }

    #region Game Stats

    /// <summary>
    /// Update the text of remaining moves.
    /// </summary>
    private void UpdateRemainingMoves()
    {
        moveCountTxt.text = GameManager.Instance.CurrentMoves.ToString();
    }

    #endregion


    #region Pause Menu

    /// <summary>
    /// Show the pause UI, timescale set to 0.
    /// </summary>
    public void Pause()
    {
        ShowResult();
        Block.canPress = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Hide pause menu, reset the timescale.
    /// </summary>
    public void Resume()
    {
        if (GameManager.Instance.CurrentMoves > 0)
        {
            Block.canPress = true;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("GAME OVER: Out of moves");
        }
    }

    /// <summary>
    /// Show the result.
    /// </summary>
    public void GameOver()
    {
        Pause();
        ShowResult();
    }

    /// <summary>
    /// Show the total points scored after the game end.
    /// </summary>
    public void ShowResult()
    {
        ShowStarEarned();
        finalPointsTxt.text = GameManager.Instance.CurrentPoints + "";
    }

    /// <summary>
    /// When total points reach a threshold, a star will be added.
    /// </summary>
    private void ShowStarEarned()
    {
        int finalPoints = GameManager.Instance.CurrentPoints;
        stars[0].SetActive(finalPoints >= GameManager.Instance.Settings.OneStarScore);
        stars[1].SetActive(finalPoints >= GameManager.Instance.Settings.TwoStarScore);
        stars[2].SetActive(finalPoints >= GameManager.Instance.Settings.ThreeStarScore);
    }

    #endregion

    #region Events
    void ConnectEvents()
    {
        GameManager.Instance.GameOver += GameOver;
        GameManager.Instance.RestartGame += UpdateRemainingMoves;
        GameManager.Instance.PlayerMadeMove += UpdateRemainingMoves;
        grid.NotEnoughBlocksToPlay += GameOver;
    }

    void DisconnectEvents()
    {
        GameManager.Instance.GameOver -= GameOver;
        GameManager.Instance.RestartGame -= UpdateRemainingMoves;
        GameManager.Instance.PlayerMadeMove -= UpdateRemainingMoves;
        grid.NotEnoughBlocksToPlay -= GameOver;
    }
    #endregion
}
