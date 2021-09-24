using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSprite : MonoBehaviour
{
    [SerializeField] TypeSprite[] spritesOfType; //This field is to easily set up types and their sprite by drag and drop in the editor.
    Dictionary<BlockType, Tuple<Sprite, Color>> blockTypes = new Dictionary<BlockType, Tuple<Sprite, Color>>(); // Dictionary of key block type and sprite value;
    BlockType type; // Current type of block

    void Awake()
    {
        InitializeBlockTypeDictionary();
    }

    /// <summary>
    /// Using dictionary to store the type and sprite of block to easily check or change type.
    /// </summary>
    void InitializeBlockTypeDictionary()
    {
        foreach (TypeSprite type in spritesOfType)
        {
            blockTypes.Add(type.type, new Tuple<Sprite, Color>(type.sprite, type.specialTint));
        }
    }

    /// <summary>
    /// Change type of block and change its sprite too.
    /// </summary>
    /// <param name="type">Type of block.</param>
    public void SetType(BlockType type)
    {
        this.type = type;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        if (blockTypes.ContainsKey(type) && !GetComponent<Block>().IsSpecial)
            sprite.sprite = blockTypes[type].Item1;

    }

    public Color GetSpecialTint()
    {
        return blockTypes[this.type].Item2;
    }


    /// <summary>
    /// Struct hold the Block Type and its sprite. Type's Sprite
    /// </summary>    
    [System.Serializable]
    public struct TypeSprite
    {
        public BlockType type;
        public Sprite sprite;
        public Color specialTint;
    }

    public int TypeCount { get => blockTypes.Count; }
    public BlockType Type { get => type; set { type = value; } }
    public Dictionary<BlockType, Tuple<Sprite, Color>> BlockTypes { get => blockTypes; set => blockTypes = value; }
}

/// <summary>
/// The types of blocks. Each block must have 1 any only 1 type.
/// </summary>
public enum BlockType
{
    MELON,
    KIWI,
    ORANGE,
    APPLE,
    PUMPKIN,
    AVOCADO,
    MUSHROOM,
    STAR,
    NONE
}
