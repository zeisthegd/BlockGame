using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int xDimension = 1;
    [SerializeField] int yDimension = 1;
    [SerializeField] GameObject piecePrefab;
    [SerializeField] GameObject[] backGroundTile;

    GamePiece[,] gamePieces;

    void Start()
    {
        InstantiatePieceBackground();
        InstantiatePieces();
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
        gamePieces = new GamePiece[xDimension, yDimension];
        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                GameObject newPiece = Instantiate(piecePrefab, new Vector2(i, j), Quaternion.identity);

                GamePiece pieceScript = newPiece.GetComponent<GamePiece>();
                pieceScript.Init(i, j, this);
                pieceScript.PieceSprite.SetType((PieceSprite.PieceType)Random.Range(0, pieceScript.PieceSprite.TypeCount));

                newPiece.transform.parent = this.transform;
                newPiece.name = pieceScript.PieceSprite.Type.ToString() + $" [{i},{j}]";

                gamePieces[i, j] = pieceScript;
            }
        }
    }

    Vector2 IndexToWorldPosition(int _x, int _y)
    {
        return new Vector2(transform.position.x - xDimension / 2 + _x, transform.position.y - yDimension / 2 + _y);
    }

}
