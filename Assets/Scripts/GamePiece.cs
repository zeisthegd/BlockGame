using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private int x;
    private int y;
    private Grid grid;
    PieceMode mode;

    PieceSprite pieceSprite;
    MoveablePiece moveableComponent;

    void Awake()
    {
        pieceSprite = GetComponent<PieceSprite>();
        moveableComponent = GetComponent<MoveablePiece>();
    }
    void Start()
    {
        pieceSprite = GetComponent<PieceSprite>();
        moveableComponent = GetComponent<MoveablePiece>();
    }
    public void Init(int _x, int _y, Grid _grid, PieceMode _mode)
    {
        x = _x;
        y = _y;
        grid = _grid;
        mode = _mode;
    }

    void OnMouseEnter()
    {
        grid.PiecesMatcher.PressedPiece(this);
        Debug.Log(Name);
    }

    public bool Moveable()
    {
        return moveableComponent != null;
    }

    public int X
    {
        get => x;
        set
        {
            if (Moveable())
                x = value;
        }
    }
    public int Y
    {
        get => y; set
        {
            if (Moveable())
                y = value;
        }
    }
    public MoveablePiece MoveableComponent { get => moveableComponent; }
    public Grid Grid { get => grid; }
    public PieceSprite PieceSprite { get => pieceSprite; }
    public PieceMode Mode { get => mode; }
    public string Name { get => pieceSprite.Type.ToString() + $" [{x},{y}]"; }
}

public enum PieceMode
{
    EMPTY,
    NORNAL
}
