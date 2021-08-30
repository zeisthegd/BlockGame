using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesMatcher : MonoBehaviour
{
    Grid grid;

    List<GamePiece> piecesToSwap;
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void PressedPiece(GamePiece piece)
    {
        if (!piecesToSwap.Contains(piece))
            piecesToSwap.Add(piece);
        else
            ReleasePiece(piece);
        if (piecesToSwap.Count == 2)
            SwapPieces(piecesToSwap[0], piecesToSwap[1]);
    }

    public void ReleasePiece(GamePiece piece)
    {
        piecesToSwap.Remove(piece);
    }

    public void SwapPieces(GamePiece pieceA, GamePiece pieceB)
    {
        if (IsAdjacent(pieceA, pieceB))
        {
            grid.Pieces[pieceA.X, pieceA.Y] = pieceB;
            grid.Pieces[pieceB.X, pieceB.Y] = pieceA;

            int pieceAX = pieceA.X;
            int pieceAY = pieceA.Y;

            pieceA.MoveableComponent.Move(pieceB.X, pieceB.Y);
            pieceB.MoveableComponent.Move(pieceAX, pieceAY);
        }
    }

    public bool IsAdjacent(GamePiece pieceA, GamePiece pieceB)
    {
        if ((Mathf.Abs(pieceA.X - pieceB.X) == 1 && pieceA.Y == pieceB.Y)
        || (Mathf.Abs(pieceA.Y - pieceB.Y) == 1) && pieceA.X == pieceB.X)
            return true;
        return false;
    }
}
