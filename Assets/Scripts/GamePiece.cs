using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private int x;
    private int y;
    private Grid grid;

    [SerializeField]PieceSprite pieceSprite;
    

    void Start()
    {
        
    }
    public void Init(int _x, int _y, Grid _grid)
    {
        x = _x;
        y = _y;
        grid = _grid;
    }

    public int X { get => x; }
    public int Y { get => y; }
    public Grid Grid { get => grid; }
    public PieceSprite PieceSprite { get => pieceSprite;}
}
