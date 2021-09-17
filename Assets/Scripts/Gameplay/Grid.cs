using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Grid : MonoBehaviour
{
    [Header("--- Grid Settings ---")]
    [SerializeField] int xDimension = 8;// X size of the board
    [SerializeField] int yDimension = 8;// Y size of the board
    [SerializeField] float fillTime = 1;// Time used to refill the board when matches are cleared.

    [Header("--- Prefabs ---")]
    [SerializeField] BlockPrefab[] blockPrefabs; // The prefabs

    [Header("--- Components ---")]
    [SerializeField] BlocksMatcher blocksMatcher;
    [SerializeField] PointCalculator pointCalculator;

    Block[,] blocks; //The array that store blocks data.
    public event UnityAction NotEnoughBlocksToPlay; //Called when there is not enough blocks to play with.

    void Awake()
    {
        blocksMatcher = GetComponent<BlocksMatcher>();
        pointCalculator = GetComponent<PointCalculator>();
    }

    void Start()
    {
        StartNewGame();
    }

    /// <summary>
    /// Reset everything and start a new game.
    /// </summary>
    public void StartNewGame()
    {
        MakeNewBoard();
        GameManager.Instance.ResetGame();
    }

    /// <summary>
    /// Clear the board and instantiate new blocks.
    /// </summary>
    public void MakeNewBoard()
    {
        ClearBoard();
        InstantiateBlocks();
        StartCoroutine(Fill());
    }

    /// <summary>
    /// Instantiate new blocks at the beginning of the game.
    /// </summary>
    void InstantiateBlocks()
    {
        blocks = new Block[xDimension, yDimension];
        Vector2 randStartPos = new Vector2(Random.Range(0, xDimension), Random.Range(0, xDimension));
        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                Block newBlock = SpawnNewBlock(i, j, BlockState.NORMAL, randStartPos);
                newBlock = MakeBlockHasNoAdjacentMatches(newBlock);
            }
        }
    }

    #region Blocks Handling
    /// <summary>
    /// Instantiate a new block at x and y position.
    /// </summary>
    /// <param name="x">X Position</param>
    /// <param name="y">Y Position</param>
    /// <param name="state">State</param>
    /// <returns></returns>
    public Block SpawnNewBlock(int x, int y, BlockState state)
    {
        return SpawnNewBlock(x, y, state, new Vector2(x, y));
    }

    /// <summary>
    /// Add a new block to the blocks array.
    /// <br/> Set up the block position, type, state.
    /// <br/> Instantiate a new block at a specific position.
    /// </summary>
    /// <param name="x">X Position</param>
    /// <param name="y">Y Position</param>
    /// <param name="state">State</param>
    /// <param name="position">Position in 2D space</param>
    /// <returns></returns>
    public Block SpawnNewBlock(int x, int y, BlockState state, Vector2 position)
    {
        GameObject newBlock = Instantiate(blockPrefabs[(int)state].prefab, position, Quaternion.identity);
        Block block = newBlock.GetComponent<Block>();
        block.Init(x, y, this, state);

        if (state == BlockState.NORMAL)
        {
            block.SetRandomBlockType();
            block.MoveableComponent.Move(block.X, block.Y);
        }
        else
        {
            block.Sprite.SetType(BlockType.NONE);
            newBlock.name = $"EMPTY [{x},{y}]";
        }


        newBlock.name = block.Sprite.Type.ToString() + $" [{block.X},{block.Y}]";
        newBlock.transform.parent = this.transform;
        blocks[x, y] = block;

        return block;
    }

    private Block MakeBlockHasNoAdjacentMatches(Block block)
    {
        var adjBlocks = block.GetAdjacentBlocks();
        foreach (Block adjBlock in adjBlocks)
        {
            if (block.Sprite.Type == adjBlock.Sprite.Type)
            {
                block.SetRandomBlockType();
                MakeBlockHasNoAdjacentMatches(block);
                return block;
            }
        }
        return block;
    }
    #endregion


    #region Board Handling
    /// <summary>
    /// After refilling the block, there will be a chance that the blocks will make more matches.
    /// <br/> So I check the board again to clear the valid matches after the filling animations has ended.
    /// </summary>
    public IEnumerator Fill()
    {
        bool needRefill = true;
        Block.CanPress = false;
        while (needRefill)
        {
            yield return new WaitForSeconds(fillTime);
            while (FillBoard())
                yield return new WaitForSeconds(fillTime);
            needRefill = blocksMatcher.ClearAllValidMatches();
            CheckCountRemainingBlocks();
        }
        Block.CanPress = true;
        GameManager.Instance.CheckRemainingMoves();
    }

    /// <summary>
    /// Fill the board with blocks.
    /// </summary>
    /// <returns>Is filling the board or not?</returns>
    public bool FillBoard()
    {
        bool filledInbetween = FillBetweenTopAndBottomRow();
        bool filledTop = FillTopRow();
        return filledInbetween || filledTop;
    }

    /// <summary>
    /// If the block at the top of the board is an EMPTY block, add a new block of a random type.
    /// </summary>
    /// <returns>Is filling top row or not?</returns>
    private bool FillTopRow()
    {
        bool filled = false;
        int topRow = yDimension - 1;
        for (int i = 0; i < xDimension; i++)
        {
            Block topBlock = blocks[i, topRow];
            if (topBlock.State == BlockState.EMPTY)
            {
                Destroy(topBlock.gameObject);
                SpawnNewBlock(i, topRow, BlockState.NORMAL, new Vector2(i, topRow + 2));
                filled = true;
            }
        }
        return filled;
    }

    /// <summary>
    /// Move a block down if the block below it is an EMPTY block.
    /// </summary>
    /// <returns>Is filling between top and bottom row or not?</returns>
    private bool FillBetweenTopAndBottomRow()
    {
        bool filled = false;
        for (int j = 1; j < yDimension; j++)
        {
            for (int i = 0; i < xDimension; i++)
            {
                Block block = blocks[i, j];
                if (block.Moveable())
                {
                    Block blockBelow = blocks[i, j - 1];
                    if (block.State != BlockState.EMPTY && blockBelow.State == BlockState.EMPTY)
                    {
                        Destroy(blockBelow.gameObject);
                        block.MoveableComponent.Move(i, j - 1);
                        blocks[i, j - 1] = block;
                        SpawnNewBlock(i, j, BlockState.EMPTY);
                        filled = true;
                    }
                }
            }
        }
        return filled;
    }



    /// <summary>
    /// Delete all the blocks gameObject on the board.
    /// </summary>
    private void ClearBoard()
    {
        StopAllCoroutines();
        var blocks = FindObjectsOfType<Block>();
        foreach (Block block in blocks)
        {
            Destroy(block.gameObject);
        }
    }

    /// <summary>
    /// Check the total count of remaining blocks
    /// I made these function because the test description requires me. 
    /// </summary>
    public void CheckCountRemainingBlocks()
    {
        int totalRemaining = CountRemainingBlocks();
        Debug.Log($"Remaing Block(s): {totalRemaining} (block)");
        if (!RemainingBlocksHasEnoughTypeToMatch())
            NotEnoughBlocksToPlay?.Invoke();
    }

    /// <summary>
    /// Count total remaining blocks
    /// </summary>
    /// <returns>Total remaining blocks</returns>
    public int CountRemainingBlocks()
    {
        int total = 0;
        foreach (Block block in this.blocks)
        {
            if (block.Sprite.Type != BlockType.NONE)
            {
                total++;
            }
        }
        return total;
    }

    /// <summary>
    /// If no type of block has more than 2 blocks, there is not enough blocks to play with.
    /// </summary>
    /// <returns>Have enough block to play with?</returns>
    public bool RemainingBlocksHasEnoughTypeToMatch()
    {
        foreach (BlockType type in blocks[0, 0].Sprite.BlockTypes.Keys)
        {
            int total = 0;
            foreach (Block block in blocks)
            {
                if (block.Sprite.Type == type)
                    total++;
                if (total > 2)
                    return true;
            }
        }
        Debug.Log("No type has enough blocks to get matched!");
        return false;
    }

    #endregion

    public int XDimension { get => xDimension; }
    public int YDimension { get => yDimension; }
    public Block[,] Blocks { get => blocks; }
    public BlocksMatcher BlocksMatcher { get => blocksMatcher; }
    public PointCalculator PointCalculator { get => pointCalculator; }

    /// <summary>
    /// Determine what prefabs that a state will use
    /// </summary>
    [System.Serializable]
    public struct BlockPrefab
    {
        public BlockState state;
        public GameObject prefab;
    }
}

