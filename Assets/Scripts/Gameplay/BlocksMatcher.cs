using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Search for matches.
/// </summary>
public class BlocksMatcher : MonoBehaviour
{
    Grid grid;// Grid that store blocks data.
    List<Block> matchedBlocks = new List<Block>();// Hold the matches that the functions have found.
    List<Block> specialBlocks = new List<Block>();// Hold the matches that the functions have found.
    List<Block> blocksToSwap = new List<Block>();// Holds the 2 blocks that is going to be swapped.

    // Add 1 of these value to X or Y position of the block to search.
    //-1 means searching left and down if add to X, 1 means searching right and up if add to Y.
    int[] checkDirections = new int[] { -1, 1 };


    public event UnityAction BeginClearingBlocks;// Called when there are matches to clear.
    public event UnityAction BeginSwappingBlocks;// Called when 2 blocks is swapped.
    public event UnityAction NewStarBlockMade;// Called a star block is formed.

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Start()
    {
        GameManager.Instance.RestartGame += Reset;
    }
    void OnDisable()
    {
        GameManager.Instance.RestartGame -= Reset;
    }

    /// <summary>
    /// Called when a block is pressed. Add the pressed block to the blocksToSwap list if it is unique.
    /// If it is not unique, it will be released.
    /// When 2 blocks have been added, they will be swapped.
    /// </summary>
    /// <param name="block">Pressed block</param>
    public void PressedBlock(Block block)
    {
        if (!blocksToSwap.Contains(block))
        {
            if (blocksToSwap.Count == 1)
                if (!blocksToSwap[0].IsAdjacent(block))
                {
                    Debug.Log("These 2 Blocks are not adjacent!");
                    return;
                }
            ChooseBlock(block);
        }
        else
            Releaseblock(block);
        if (blocksToSwap.Count >= 2)
        {
            SwapBlocks(blocksToSwap[0], blocksToSwap[1]);
            ClearAllValidMatches();
            grid.StartFillingBoard();
        }
    }

    ///<summary>
    /// Add the block to the blocksToSwap list, show the chosen UI sprite of the block
    ///</summary>
    ///<param name="block">Block</params>
    public void ChooseBlock(Block block)
    {
        blocksToSwap.Add(block);
        block.ActiveChosenUI();
    }

    ///<summary>
    /// Remove the block to the blocksToSwap list, hide the chosen UI sprite of the block
    ///</summary>
    ///<param name="block">Block</params>
    public void Releaseblock(Block block)
    {
        blocksToSwap.Remove(block);
        block.DeactiveChosenUI();
    }

    ///<summary>
    /// Swap chosen blocks positions if their new positions make matches.Finally, clear the blocksToSwap list.
    ///</summary>
    ///<param name="blockA">Block A</params>
    ///<param name="blockB">Block B</params>
    public void SwapBlocks(Block blockA, Block blockB)
    {
        BeginSwappingBlocks?.Invoke();
        if (blockA.IsAdjacent(blockB))
        {
            bool bothAandBNotEmpty = blockA.Sprite.Type != BlockType.NONE && blockB.Sprite.Type != BlockType.NONE;
            SwapPositions(blockA, blockB);

            var blocksFromA = GetMatch(blockA);
            var blocksFromB = GetMatch(blockB);

            if (blocksFromA.Count >= 3 || blocksFromB.Count >= 3 && bothAandBNotEmpty)
            {
                AddFoundBlocksTo(matchedBlocks, blocksFromA);
                AddFoundBlocksTo(matchedBlocks, blocksFromB);

                if (blocksFromA.Count >= 4)
                    AddBlockToSpecial(blockA);
                if (blocksFromB.Count >= 4)
                    AddBlockToSpecial(blockB);
            }
            else
                SwapPositions(blockB, blockA);
        }
        blocksToSwap.Clear();
    }

    ///<summary>
    /// Swap 2 blocks position in 3D space and in grid data.
    ///</summary>
    ///<param name="blockA">Block A</params>
    ///<param name="blockB">Block B</params>
    private void SwapPositions(Block blockA, Block blockB)
    {
        int blockAX = blockA.X;
        int blockAY = blockA.Y;
        grid.Blocks[blockA.X, blockA.Y] = blockB;
        grid.Blocks[blockB.X, blockB.Y] = blockA;
        blockA.MoveableComponent.Move(blockB.X, blockB.Y);
        blockB.MoveableComponent.Move(blockAX, blockAY);
    }

    ///<summary>
    /// Search through all the blocks of the grid, find matches to clear.
    ///</summary>
    public bool ClearAllValidMatches()
    {
        bool cleared = false;
        for (int i = 0; i < grid.XDimension; i++)
        {
            for (int j = 0; j < grid.YDimension; j++)
            {
                if (grid.Blocks[i, j].ClearableComponent != null && !matchedBlocks.Contains(grid.Blocks[i, j]))
                {
                    List<Block> matches = GetMatch(grid.Blocks[i, j]);
                    if (matches.Count >= 3)
                    {
                        AddFoundBlocksTo(matchedBlocks, matches);
                        cleared = true;
                        if (matches.Count >= 4)
                            AddBlockToSpecial(matches[matches.Count / 2]);
                    }
                }
            }
        }
        if (matchedBlocks.Count >= 3)
            BeginClearingBlocks?.Invoke();

        /// Clear all matches.
        foreach (Block block in matchedBlocks)
        {
            if (!specialBlocks.Contains(block))
                block.ClearableComponent.Clear();
        }
        /// Change the blocks in specialblocks to the special type.
        foreach (Block block in specialBlocks)
        {
            block.SetSpecialType();
            NewStarBlockMade?.Invoke();
        }

        matchedBlocks.Clear();
        specialBlocks.Clear();

        return cleared;
    }

    /// <summary>
    /// Get all blocks of the same type of the chosen blocks in all 4 directions
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    public List<Block> GetMatch(Block block)
    {
        List<Block> matchingBlocks = new List<Block>();
        if (block.State == BlockState.NORMAL)
        {
            BlockType type = block.Sprite.Type;
            List<Block> horizontalBlocks = GetHorizontalBlocks(block);
            List<Block> verticalBlocks = GetVerticalBlocks(block);

            if (horizontalBlocks.Count >= 3)
                AddFoundBlocksTo(matchingBlocks, horizontalBlocks);
            if (verticalBlocks.Count >= 3)
                AddFoundBlocksTo(matchingBlocks, verticalBlocks);
        }
        return matchingBlocks;
    }

    /// <summary>
    /// Get all blocks of the same type of the chosen blocks in 2 directions: Left and Right.
    /// </summary>
    /// <param name="block">Chosen block</param>
    /// <returns></returns>
    private List<Block> GetHorizontalBlocks(Block block)
    {
        List<Block> horizontalBlocks = new List<Block>();

        horizontalBlocks.Add(block);
        var leftBlocks = GetBlocksInDirection(block, checkDirections[0], checkDirections[0], 0, block.Y);
        var rightBlocks = GetBlocksInDirection(block, checkDirections[1], checkDirections[1], grid.XDimension - 1, block.Y);

        AddFoundBlocksTo(horizontalBlocks, leftBlocks);
        AddFoundBlocksTo(horizontalBlocks, rightBlocks);
        return horizontalBlocks;
    }

    /// <summary>
    /// Get all blocks of the same type of the chosen blocks in 2 directions: Up and Down.
    /// </summary>
    /// <param name="block">Chosen block</param>
    /// <returns></returns>
    private List<Block> GetVerticalBlocks(Block block)
    {
        List<Block> verticalBlocks = new List<Block>();

        verticalBlocks.Add(block);
        var belowBlocks = GetBlocksInDirection(block, checkDirections[0], checkDirections[0], block.X, 0);
        var aboveBlocks = GetBlocksInDirection(block, checkDirections[1], checkDirections[1], block.X, grid.YDimension - 1);

        AddFoundBlocksTo(verticalBlocks, belowBlocks);
        AddFoundBlocksTo(verticalBlocks, aboveBlocks);

        return verticalBlocks;
    }

    /// <summary>
    /// Add a block list to another list. Except the duplicate blocks.
    /// </summary>
    /// <param name="blockLists">Original list.</param>
    /// <param name="otherBlocks">Other list</param>
    private void AddFoundBlocksTo(List<Block> blockLists, List<Block> otherBlocks)
    {
        foreach (Block block in otherBlocks)
        {
            if (!blockLists.Contains(block))
            {
                blockLists.Add(block);
            }
        }
    }

    /// <summary>
    /// Add the block to special blocks list.
    /// </summary>
    /// <param name="block">Block</param>
    private void AddBlockToSpecial(Block block)
    {
        if (!specialBlocks.Contains(block))
            specialBlocks.Add(block);
    }


    /// <summary>
    /// Get blocks of the same type of a chosen block in 1 direction.
    /// </summary>
    /// <param name="block">Chosen block.</param>
    /// <param name="xDir">X direction</param>
    /// <param name="yDir">Y direction</param>
    /// <param name="endX">The stopping X position</param>
    /// <param name="endY">The stopping Y position</param>
    /// <returns>Lists of found blocks.</returns>
    private List<Block> GetBlocksInDirection(Block block, int xDir, int yDir, int endX, int endY)
    {
        List<Block> blocks = new List<Block>();
        for (int i = block.X; i != endX + xDir; i += xDir)
        {
            for (int j = block.Y; j != endY + yDir; j += yDir)
            {
                Block foundBlock = grid.Blocks[i, j];
                if ((foundBlock.Sprite.Type == BlockType.STAR || foundBlock.Sprite.Type == block.Sprite.Type)
                    && !blocks.Contains(foundBlock))
                {
                    blocks.Add(foundBlock);
                }
                else
                    return blocks;
            }
        }
        return blocks;
    }

    /// <summary>
    /// Search through every block of the grid, try to swap them with their adjacent blocks.
    /// If by swapping they make new match, swap them back and return true.
    /// </summary>
    /// <returns>Does the board have any match to make?</returns>
    public bool HasMatchesToMake()
    {
        foreach (Block block in grid.Blocks)
        {
            var adjBlocks = block.GetAdjacentBlocks();
            foreach (Block adjBlock in adjBlocks)
            {
                var ogType = adjBlock.Sprite.Type;
                adjBlock.Sprite.Type = block.Sprite.Type;
                block.Sprite.Type = ogType;

                if (GetMatch(adjBlock).Count >= 3)
                {
                    block.Sprite.Type = adjBlock.Sprite.Type;
                    adjBlock.Sprite.Type = ogType;
                    return true;
                }

                block.Sprite.Type = adjBlock.Sprite.Type;
                adjBlock.Sprite.Type = ogType;
            }
        }
        Debug.Log("Don't have enough blocks to match");
        return false;
    }



    /// <summary>
    /// Stop all coroutine and clear the block lists.
    /// </summary>
    public void Reset()
    {
        StopAllCoroutines();
        matchedBlocks.Clear();
        blocksToSwap.Clear();
        specialBlocks.Clear();
    }
    public List<Block> MatchedBlocks { get => matchedBlocks; }

}
