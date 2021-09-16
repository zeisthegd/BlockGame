using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameSettings settings;

    int currentMoves;// Total moves left.
    int currentPoints; // Total points the player has scored.

    public event UnityAction GameOver;// Called when there is no move left.
    public event UnityAction RestartGame;// Invoke when the game is restarted.
    public event UnityAction PlayerMadeMove;// Called when the player swap the blocks.

    public static GameManager Instance = null; // Unique instance of game manager


    void Awake()
    {
        grid = FindObjectOfType<Grid>();
        ResetGame();
        MakeSingleton();
    }

    void OnEnable()
    {
        grid.BlocksMatcher.BeginSwappingBlocks += MakeMove;
    }
    void OnDisable()
    {
        grid.BlocksMatcher.BeginSwappingBlocks -= MakeMove;
    }

    /// <summary>
    /// When the player make moves, total moves left is decreased.
    /// </summary>
    private void MakeMove()
    {
        currentMoves -= 1;
        PlayerMadeMove?.Invoke();
    }

    /// <summary>
    /// Check if the player has run out of moves or not.
    /// </summary>
    public void CheckRemainingMoves()
    {
        if (currentMoves <= 0)
        {
            GameOver?.Invoke();
        }
    }

    /// <summary>
    /// Reset the points and moves.
    /// </summary>
    public void ResetGame()
    {
        currentPoints = 0;
        currentMoves = settings.Moves;
        RestartGame?.Invoke();
    }

    /// <summary>
    /// Because game manager is unique for every game, it can only have one instance.
    /// </summary>
    private void MakeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public int CurrentPoints { get => currentPoints; set => currentPoints = value; }
    public int CurrentMoves { get => currentMoves; private set => currentMoves = value; }
    public GameSettings Settings { get => settings; private set => settings = value; }
}
