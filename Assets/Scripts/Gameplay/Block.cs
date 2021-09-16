using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static bool canPress = true; // Can the block be pressed or not? When refilling the board, stop the player from pressing the blocks.
    private int x; // X position of the block in the blocks array.
    private int y; // Y position of the block in the blocks array.
    private Grid grid; // Grid that holds blocks' position data.
    BlockState state; //Current state of a block

    BlockSprite blockSprite;
    MoveableBlock moveableComponent;
    ClearableBlock clearableComponent;

    [SerializeField] SpriteRenderer chosenSprite; // The sprite will appear when the block is chosen


    /// <summary>
    /// Get components.
    /// </summary>
    void Awake()
    {
        blockSprite = GetComponent<BlockSprite>();
        moveableComponent = GetComponent<MoveableBlock>();
        clearableComponent = GetComponent<ClearableBlock>();
    }

    /// <summary>
    /// Set up the block position and state.
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    /// <param name="grid">Grid</param>
    /// <param name="state">State</param>
    public void Init(int x, int y, Grid grid, BlockState state)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
        this.state = state;
    }

    ///<summary>
    /// Called when the block is clicked.
    ///</summary>
    void OnMouseDown()
    {
        if (canPress)
            grid.BlocksMatcher.PressedBlock(this);
    }

    public bool Moveable()
    {
        return moveableComponent != null;
    }

    ///<summary>
    /// Make the chosen UI sprite appear under the block.
    ///</summary>
    public void ActiveChosenUI()
    {
        if (ChosenSprite != null)
            ChosenSprite.gameObject.SetActive(true);
    }

    ///<summary>
    /// Make the chosen UI sprite disappear.
    ///</summary>
    public void DeactiveChosenUI()
    {
        if (ChosenSprite != null)
            ChosenSprite.gameObject.SetActive(false);
    }

    /// <summary>
    /// X position, only set if moveable.
    /// </summary>
    /// <value></value>
    public int X
    {
        get => x;
        set
        {
            if (Moveable())
                x = value;
        }
    }
    /// <summary>
    /// Y position, only set if moveable.
    /// </summary>
    /// <value></value>
    public int Y
    {
        get => y; set
        {
            if (Moveable())
                y = value;
        }
    }
    public Grid Grid { get => grid; }
    public BlockState State { get => state; }
    public BlockSprite Sprite { get => blockSprite; }
    public SpriteRenderer ChosenSprite { get => chosenSprite; }
    public MoveableBlock MoveableComponent { get => moveableComponent; }
    public string Name { get => Sprite.Type.ToString() + $" [{x},{y}]"; }
    public ClearableBlock ClearableComponent { get => clearableComponent; set => clearableComponent = value; }
}

///<summary>
/// EMPTY means cleared, NORMAL is when the block has sprite.
/// The NORMAL blocks can drop down if it is above an EMPTY block.
///</summary>
/// <value>NORMAL, EMPTY</value>
public enum BlockState
{
    EMPTY,
    NORMAL
}
