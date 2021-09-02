using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static bool canPress = true;
    private int x;
    private int y;
    private Grid grid;
    BlockMode mode;

    BlockSprite blockSprite;
    ClearableBlock clearableComponent;
    MoveableBlock moveableComponent;

    [SerializeField] SpriteRenderer chosenSprite;

    void Awake()
    {
        blockSprite = GetComponent<BlockSprite>();
        moveableComponent = GetComponent<MoveableBlock>();
        clearableComponent = GetComponent<ClearableBlock>();
    }
    void Start()
    {

    }
    public void Init(int _x, int _y, Grid _grid, BlockMode _mode)
    {
        x = _x;
        y = _y;
        grid = _grid;
        mode = _mode;
    }

    void OnMouseDown()
    {
        if(canPress)
            grid.BlocksMatcher.PressedBlock(this);
    }

    public bool Moveable()
    {
        return moveableComponent != null;
    }

    public void ActiveChosenUI()
    {
        ChosenSprite.gameObject.SetActive(true);
    }
    public void DeactiveChosenUI()
    {
        ChosenSprite.gameObject.SetActive(false);
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
    public Grid Grid { get => grid; }
    public BlockMode Mode { get => mode; }
    public BlockSprite Sprite { get => blockSprite; }
    public SpriteRenderer ChosenSprite { get => chosenSprite; }
    public MoveableBlock MoveableComponent { get => moveableComponent; }
    public string Name { get => Sprite.Type.ToString() + $" [{x},{y}]"; }
    public ClearableBlock ClearableComponent { get => clearableComponent; set => clearableComponent = value; }
}

public enum BlockMode
{
    EMPTY,
    NORMAL
}
