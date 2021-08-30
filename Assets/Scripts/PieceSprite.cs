using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSprite : MonoBehaviour
{
    [SerializeField] TypeSprites[] typeSprites;
    Dictionary<PieceType, Sprite> pieceTypes = new Dictionary<PieceType, Sprite>();
    PieceType type;




    void Awake()
    {
        InitializePieceTypeDictionary();
    }

    void InitializePieceTypeDictionary()
    {
        foreach (TypeSprites _type in typeSprites)
        {
            pieceTypes.Add(_type.type, _type.sprite);
        }
    }

    public void SetType(PieceType _type)
    {
        type = _type;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        if (pieceTypes.ContainsKey(type))
            sprite.sprite = pieceTypes[type];
    }

    [System.Serializable]
    struct TypeSprites
    {
        public PieceType type;
        public Sprite sprite;
    }

    public PieceType Type { get => type; set { SetType(value); } }
    public int TypeCount { get => pieceTypes.Count; }

    public enum PieceType
    {
        SANDWICH,
        BURRITO,
        CHEESECAKE,
        JELLY,
        STEAK,
        ROASTEDCHICKEN,
        APPLEPIE
    }
}
