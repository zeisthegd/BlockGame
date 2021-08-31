﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksMatcher : MonoBehaviour
{
    Grid grid;
    List<Block> blocksToSwap = new List<Block>();
    int[] checkDirections = new int[] { -1, 1 };

    List<Block> matchedBlocks = new List<Block>();
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void PressedBlock(Block block)
    {
        if (!blocksToSwap.Contains(block))
        {
            if (blocksToSwap.Count == 1)
                if (!IsAdjacent(blocksToSwap[0], block))
                {
                    Debug.Log("2 Blocks not adjacent!");
                    return;
                }
            Chooseblock(block);
        }
        else
            Releaseblock(block);
        if (blocksToSwap.Count == 2)
        {
            SwapBlocks(blocksToSwap[0], blocksToSwap[1]);
            ClearMatchedBlocks();
            StartCoroutine(grid.Fill());
        }
    }
    public void Chooseblock(Block block)
    {
        blocksToSwap.Add(block);
        block.ActiveChosenUI();
    }
    public void Releaseblock(Block block)
    {
        blocksToSwap.Remove(block);
        block.DeactiveChosenUI();
    }

    public void SwapBlocks(Block _blockA, Block _blockB)
    {
        if (IsAdjacent(_blockA, _blockB))
        {
            //Swap board data and position
            grid.Blocks[_blockA.X, _blockA.Y] = _blockB;
            grid.Blocks[_blockB.X, _blockB.Y] = _blockA;

            int blockAX = _blockA.X;
            int blockAY = _blockA.Y;
            _blockA.MoveableComponent.Move(_blockB.X, _blockB.Y);
            _blockB.MoveableComponent.Move(blockAX, blockAY);

            blocksToSwap.Clear();
            var matchedABlocks = GetMatch(_blockA, _blockA.X, _blockA.Y);
            var matchedBBlocks = GetMatch(_blockB, _blockB.X, _blockB.Y);

            if (matchedABlocks.Count >= 3)
            {
                AddFoundBlocksTo(matchedBlocks, matchedABlocks);
                if (matchedBBlocks.Count >= 3)
                    AddFoundBlocksTo(matchedBlocks, matchedBBlocks);
            }
            else
            {
                //Reset board data and position
                blockAX = _blockA.X;
                blockAY = _blockA.Y;
                _blockA.MoveableComponent.Move(_blockB.X, _blockB.Y);
                _blockB.MoveableComponent.Move(blockAX, blockAY);
                grid.Blocks[_blockA.X, _blockA.Y] = _blockA;
                grid.Blocks[_blockB.X, _blockB.Y] = _blockB;
            }
        }
    }

    public void ClearMatchedBlocks()
    {
        foreach (Block block in matchedBlocks)
        {
            Debug.Log(block.Name);
            block.ClearableComponent.Clear();
        }
    }

    public List<Block> GetMatch(Block _block, int _newX, int _newY)
    {
        Debug.Log("Finding at: " + _block.Name);
        List<Block> matchingBlocks = new List<Block>();
        if (_block.Mode == BlockMode.NORMAL)
        {
            BlockType type = _block.Sprite.Type;
            List<Block> horizontalBlocks = GetHorizontalBlocks(_block);
            List<Block> verticalBlocks = GetVerticalBlocks(_block);

            AddFoundBlocksTo(matchingBlocks, horizontalBlocks);
            AddFoundBlocksTo(matchingBlocks, verticalBlocks);
        }
        return matchingBlocks;
    }

    private List<Block> GetHorizontalBlocks(Block _block)
    {
        List<Block> horizontalBlocks = new List<Block>();

        horizontalBlocks.Add(_block);
        var leftBlocks = GetBlocksInDirection(_block, checkDirections[0], checkDirections[0], 0, _block.Y);
        var rightBlocks = GetBlocksInDirection(_block, checkDirections[1], checkDirections[1], grid.XDimension - 1, _block.Y);

        AddFoundBlocksTo(horizontalBlocks, leftBlocks);
        AddFoundBlocksTo(horizontalBlocks, rightBlocks);
        return horizontalBlocks;
    }
    private List<Block> GetVerticalBlocks(Block _block)
    {
        List<Block> verticalBlocks = new List<Block>();

        verticalBlocks.Add(_block);
        var belowBlocks = GetBlocksInDirection(_block, checkDirections[0], checkDirections[0], _block.X, 0);
        var aboveBlocks = GetBlocksInDirection(_block, checkDirections[1], checkDirections[1], _block.X, grid.YDimension - 1);

        AddFoundBlocksTo(verticalBlocks, belowBlocks);
        AddFoundBlocksTo(verticalBlocks, aboveBlocks);

        return verticalBlocks;
    }

    private void AddFoundBlocksTo(List<Block> _blockLists, List<Block> _otherBlocks)
    {
        foreach (Block block in _otherBlocks)
        {
            if (!_blockLists.Contains(block))
            {
                _blockLists.Add(block);
            }
        }
    }

    private List<Block> GetBlocksInDirection(Block _block, int _xDir, int _yDir, int _endX, int _endY)
    {
        List<Block> blocks = new List<Block>();
        for (int i = _block.X; i != _endX + _xDir; i += _xDir)
        {
            for (int j = _block.Y; j != _endY + _yDir; j += _yDir)
            {
                Block foundBlock = grid.Blocks[i, j];
                if (foundBlock.Sprite.Type == _block.Sprite.Type && !blocks.Contains(foundBlock))
                {
                    blocks.Add(foundBlock);
                }
            }
        }
        return blocks;
    }

    public bool IsAdjacent(Block _blockA, Block _blockB)
    {
        if ((Mathf.Abs(_blockA.X - _blockB.X) == 1 && _blockA.Y == _blockB.Y)
        || (Mathf.Abs(_blockA.Y - _blockB.Y) == 1) && _blockA.X == _blockB.X)
            return true;
        return false;
    }
}
