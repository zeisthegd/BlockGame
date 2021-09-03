using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSprite : MonoBehaviour
{
    [SerializeField] TypeSprites[] typeSprites;
    Dictionary<BlockType, Sprite> blockTypes = new Dictionary<BlockType, Sprite>();
    BlockType type;
    void Awake()
    {
        InitializeBlockTypeDictionary();
    }

    void InitializeBlockTypeDictionary()
    {
        foreach (TypeSprites _type in typeSprites)
        {
            blockTypes.Add(_type.type, _type.sprite);
        }
    }

    public void SetType(BlockType _type)
    {
        type = _type;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        if (blockTypes.ContainsKey(type))
            sprite.sprite = blockTypes[type];
    }

    [System.Serializable]
    struct TypeSprites
    {
        public BlockType type;
        public Sprite sprite;
    }

    public BlockType Type { get => type; set { SetType(value); } }
    public int TypeCount { get => blockTypes.Count; }
    public Dictionary<BlockType, Sprite> BlockTypes { get => blockTypes; set => blockTypes = value; }
}

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
