using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int xDimension = 1;
    [SerializeField] int yDimension = 1;
    [SerializeField] float fillTime = 1;
    [SerializeField] PiecePrefab[] piecePrefabs;
    [SerializeField] GameObject[] backGroundTile;

    GamePiece[,] pieces;
    PiecesMatcher piecesMatcher;


    void Start()
    {
        InstantiatePieceBackground();
        StartNewGame();
    }

    public void StartNewGame()
    {
        ClearBoard();
        InstantiatePieces();
        StartCoroutine(Fill());
    }

    void InstantiatePieceBackground()
    {
        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                GameObject bgTile = Instantiate(backGroundTile[((i + j) % 2 == 0) ? 0 : 1], new Vector2(i, j), Quaternion.identity);
                bgTile.transform.parent = this.transform.Find("BG");
            }
        }
    }

    void InstantiatePieces()
    {
        pieces = new GamePiece[xDimension, yDimension];
        Vector2 randStartPos = new Vector2(Random.Range(0, xDimension), Random.Range(0, xDimension));
        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                SpawnNewPiece(i, j, PieceMode.EMPTY);
            }
        }
    }

    public IEnumerator Fill()
    {
        while (FillBoard())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }
    public bool FillBoard()
    {
        bool filledInbetween = FillBetweenTopAndBottomRow();
        bool filledTop = FillTopRow();
        return filledInbetween || filledTop;
    }
    private bool FillTopRow()
    {
        bool filled = false;
        int topRow = yDimension - 1;
        for (int i = 0; i < xDimension; i++)
        {
            GamePiece topPiece = pieces[i, topRow];
            if (topPiece.Mode == PieceMode.EMPTY)
            {
                Destroy(topPiece.gameObject);
                SpawnNewPiece(i, topRow, PieceMode.NORNAL, new Vector2(i, topRow + 2));
                filled = true;
            }
        }
        return filled;
    }

    private bool FillBetweenTopAndBottomRow()
    {
        bool filled = false;
        for (int j = 1; j < yDimension; j++)
        {
            for (int i = 0; i < xDimension; i++)
            {
                GamePiece piece = pieces[i, j];
                if (piece.Moveable())
                {
                    GamePiece pieceBelow = pieces[i, j - 1];
                    if (pieceBelow.Mode == PieceMode.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MoveableComponent.Move(i, j - 1);
                        pieces[i, j - 1] = piece;
                        SpawnNewPiece(i, j, PieceMode.EMPTY);
                        filled = true;
                    }
                }
            }
        }
        return filled;
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceMode mode)
    {
        return SpawnNewPiece(x, y, mode, new Vector2(x, y));
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceMode mode, Vector2 position)
    {
        GameObject newPiece = Instantiate(piecePrefabs[(int)mode].prefab, position, Quaternion.identity);
        GamePiece pieceScript = newPiece.GetComponent<GamePiece>();
        pieceScript.Init(x, y, this, mode);

        if (mode == PieceMode.NORNAL)
        {
            pieceScript.PieceSprite.SetType((PieceSprite.PieceType)Random.Range(0, pieceScript.PieceSprite.TypeCount));
            pieceScript.MoveableComponent.Move(pieceScript.X, pieceScript.Y);
            newPiece.name = pieceScript.PieceSprite.Type.ToString() + $" [{x},{y}]";
        }
        else newPiece.name = $"EMPTY [{x},{y}]";

        newPiece.transform.parent = this.transform;
        pieces[x, y] = pieceScript;

        return pieceScript;
    }

    private void ClearBoard()
    {
        var pieces = FindObjectsOfType<GamePiece>();
        foreach (GamePiece piece in pieces)
        {
            Destroy(piece.gameObject);
        }
    }

    public PiecesMatcher PiecesMatcher { get => piecesMatcher; }
    public GamePiece[,] Pieces { get => pieces;  }

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceMode mode;
        public GameObject prefab;
    }



}

