using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    protected Block block;
    protected BlocksMatcher blocksMatcher;

    public virtual List<Block> GetBlocksUsingBlockEffect() { return null; }

    public static BlockEffect CreateNewBlockEffect(BlockEffectCode code, Block block)
    {
        BlockEffect newEffect = new NoEffect();
        switch (code)
        {
            case BlockEffectCode.ClearHorizontalLine:
                break;
            case BlockEffectCode.ClearVerticalLine:
                break;
        }
        return newEffect;
    }
}

public enum BlockEffectCode
{
    ClearHorizontalLine,
    ClearVerticalLine,
    NoEffect
}
