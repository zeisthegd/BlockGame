using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePiece : MonoBehaviour
{
    [SerializeField] float moveAnimDuration;
    GamePiece piece;

    void Awake()
    {
        piece = GetComponent<GamePiece>();
    }
    void Start()
    {

    }

    public void Move(int _newX, int _newY)
    {
        StartCoroutine(MoveCoroutine(_newX, _newY));
    }

    IEnumerator MoveCoroutine(int _newX, int _newY)
    {
        piece.gameObject.name = piece.PieceSprite.Type.ToString() + $" [{_newX},{_newY}]"; 
        piece.X = _newX;
        piece.Y = _newX;

        float time = 0;
        while (time <= 1)
        {
            yield return null;
            time += Time.deltaTime * 1 / moveAnimDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(_newX, _newY, 0), time);
        }
    }
}
