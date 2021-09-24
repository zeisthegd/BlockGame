using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    protected Block block;
    protected BlocksMatcher blocksMatcher;

    void Start()
    {
        block = GetComponent<Block>();
        blocksMatcher = FindObjectOfType<BlocksMatcher>();
    }

    public List<Block> GetBlocksUsingBlockEffect()
    {
        List<Block> foundBlocks = new List<Block>();
        switch (block.Data.Type)
        {
            case BlockType.PUMPKIN:
                foundBlocks = FindAllHorizontalBlocks();
                break;
            case BlockType.AVOCADO:
                foundBlocks = FindAllVerticalBlocks();
                break;
        }
        return foundBlocks;
    }

    private List<Block> FindAllHorizontalBlocks()
    {
        return blocksMatcher.GetHorizontalBlocks(block, true);
    }
    private List<Block> FindAllVerticalBlocks()
    {
        return blocksMatcher.GetVerticalBlocks(block, true);
    }
}

public enum BlockEffectCode
{
    ClearHorizontalLine,
    ClearVerticalLine,
    NoEffect
}
