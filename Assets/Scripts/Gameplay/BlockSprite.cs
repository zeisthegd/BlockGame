using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSprite : MonoBehaviour
{
    [SerializeField] TypeSprite[] typeSprites; //This field is to easily set up types and their sprite by drag and drop in the editor.
    Dictionary<BlockType, Sprite> blockTypes = new Dictionary<BlockType, Sprite>(); // Dictionary of key block type and sprite value;
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
        foreach (TypeSprite type in typeSprites)
        {
            blockTypes.Add(type.type, type.sprite);
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
        if (blockTypes.ContainsKey(type))
            sprite.sprite = blockTypes[type];
    }


    /// <summary>
    /// Struct hold the Block Type and its sprite.
    /// </summary>    
    [System.Serializable]
    struct TypeSprite
    {
        public BlockType type;
        public Sprite sprite;
    }

    public int TypeCount { get => blockTypes.Count; }
    public BlockType Type { get => type; set { SetType(value); } }
    public Dictionary<BlockType, Sprite> BlockTypes { get => blockTypes; set => blockTypes = value; }
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
    NONE
}
