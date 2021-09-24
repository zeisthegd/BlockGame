using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static bool CanPress = true; // Can the block be pressed or not? When refilling the board, stop the player from pressing the blocks.
    private int x; // X position of the block in the blocks array.
    private int y; // Y position of the block in the blocks array.
    private bool isSpecial; // True if this is a Star block.
    private Grid grid; // Grid that holds blocks' position data.
    BlockState state; //Current state of a block\
    BlockEffect effect;

    BlockData blockData;
    MoveableBlock moveableComponent;
    ClearableBlock clearableComponent;

    [SerializeField] SpriteRenderer chosenSprite; // The sprite will appear when the block is chosen


    /// <summary>
    /// Get components.
    /// </summary>
    void Awake()
    {
        blockData = GetComponent<BlockData>();
        moveableComponent = GetComponent<MoveableBlock>();
        clearableComponent = GetComponent<ClearableBlock>();
        effect = GetComponent<BlockEffect>();
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

    /// <summary>
    /// Change block type.
    /// </summary>
    /// <param name="block"></param>
    public void SetRandomBlockType()
    {
        this.Data.SetType((BlockType)Random.Range(0, 7));
        this.gameObject.name = Name;
    }

    /// <summary>
    /// Change the sprite to special sprite.
    /// <br/> Change the tint color to match its fruit type.
    /// <br/> Play the special effect.
    /// </summary>
    public void MakeSpecialType()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = this.Data.BlockTypes[BlockType.STAR].Item1;
        sprite.material.color = Data.GetSpecialTint();
        GetComponent<BlockVisualFX>().PlaySpecialEffect();
        isSpecial = true;
    }

    /// <summary>
    /// Check if the other block is adjacent to this block.
    /// </summary>
    /// <param name="blockB">Other Block</param>
    /// <returns>The other blocks is adjacent or not?</returns>
    public bool IsAdjacent(Block blockB)
    {
        if ((Mathf.Abs(this.X - blockB.X) == 1 && this.Y == blockB.Y)
        || (Mathf.Abs(this.Y - blockB.Y) == 1) && this.X == blockB.X)
            return true;
        return false;
    }

    /// <summary>
    /// Get the 4 blocks in 4 directions: Up, Down, Left, Right
    /// </summary>
    /// <returns>Adjacent Blocks</returns>
    public List<Block> GetAdjacentBlocks()
    {
        List<Block> adjBlocks = new List<Block>();

        if (X > 0 && grid.Blocks[X - 1, Y] != null)
            adjBlocks.Add(grid.Blocks[X - 1, Y]);

        if (X < grid.XDimension - 1 && grid.Blocks[X + 1, Y] != null)
            adjBlocks.Add(grid.Blocks[X + 1, Y]);

        if (y > 0 && grid.Blocks[X, Y - 1] != null)
            adjBlocks.Add(grid.Blocks[X, Y - 1]);

        if (Y < grid.YDimension - 1 && grid.Blocks[X, Y + 1] != null)
            adjBlocks.Add(grid.Blocks[X, Y + 1]);

        return adjBlocks;
    }

    ///<summary>
    /// Called when the block is clicked.
    ///</summary>
    void OnMouseDown()
    {
        if (CanPress)
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
    public BlockData Data { get => blockData; }
    public SpriteRenderer ChosenSprite { get => chosenSprite; }
    public string Name { get => Data.Type.ToString() + $" [{x},{y}]"; }
    public bool IsSpecial { get => isSpecial; }
    public MoveableBlock MoveableComponent { get => moveableComponent; }

    public ClearableBlock ClearableComponent { get => clearableComponent; set => clearableComponent = value; }
    public BlockEffect Effect { get => effect; set => effect = value; }
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
