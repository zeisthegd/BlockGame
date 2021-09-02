using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlocksMatcher : MonoBehaviour
{
    Grid grid;
    int[] checkDirections = new int[] { -1, 1 };
    List<Block> matchedBlocks = new List<Block>();
    List<Block> blocksToSwap = new List<Block>();


    public event UnityAction BeginClearingBlocks;
    public event UnityAction BeginSwappingBlocks;

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
            ClearAllValidMatches();
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
        BeginSwappingBlocks?.Invoke();
        if (IsAdjacent(_blockA, _blockB))
        {
            grid.Blocks[_blockA.X, _blockA.Y] = _blockB;
            grid.Blocks[_blockB.X, _blockB.Y] = _blockA;

            int blockAX = _blockA.X;
            int blockAY = _blockA.Y;
            _blockA.MoveableComponent.Move(_blockB.X, _blockB.Y);
            _blockB.MoveableComponent.Move(blockAX, blockAY);

            blocksToSwap.Clear();
            if (GetMatch(_blockA, _blockA.X, _blockA.Y).Count < 3 && GetMatch(_blockB, _blockB.X, _blockB.Y).Count < 3)
            {
                blockAX = _blockA.X;
                blockAY = _blockA.Y;
                _blockA.MoveableComponent.Move(_blockB.X, _blockB.Y);
                _blockB.MoveableComponent.Move(blockAX, blockAY);
                grid.Blocks[_blockA.X, _blockA.Y] = _blockA;
                grid.Blocks[_blockB.X, _blockB.Y] = _blockB;
            }
        }
    }

    public bool ClearAllValidMatches()
    {
        bool cleared = false;
        matchedBlocks.Clear();
        for (int i = 0; i < grid.XDimension; i++)
        {
            for (int j = 0; j < grid.YDimension; j++)
            {
                if (grid.Blocks[i, j].ClearableComponent != null)
                {
                    List<Block> matches = GetMatch(grid.Blocks[i, j], i, j);
                    if (matches.Count >= 3)
                    {
                        AddFoundBlocksTo(matchedBlocks, matches);
                        cleared = true;
                    }
                }
            }
        }
        if (cleared)
            BeginClearingBlocks?.Invoke();
        foreach (Block block in matchedBlocks)
        {
            block.ClearableComponent.Clear();
        }
        return cleared;
    }

    public List<Block> GetMatch(Block _block, int _newX, int _newY)
    {
        List<Block> matchingBlocks = new List<Block>();
        if (_block.Mode == BlockMode.NORMAL)
        {
            BlockType type = _block.Sprite.Type;
            List<Block> horizontalBlocks = GetHorizontalBlocks(_block);
            List<Block> verticalBlocks = GetVerticalBlocks(_block);

            if (horizontalBlocks.Count >= 3)
                AddFoundBlocksTo(matchingBlocks, horizontalBlocks);
            if (verticalBlocks.Count >= 3)
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
                else
                    return blocks;
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

    public void Reset()
    {
        matchedBlocks.Clear();
        blocksToSwap.Clear();
    }
    public List<Block> MatchedBlocks { get => matchedBlocks; }

}
