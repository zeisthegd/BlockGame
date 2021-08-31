using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour
{
    [Header("--- Animation ---")]
    [SerializeField] float moveAnimDuration;
    Block block;

    void Awake()
    {
        block = GetComponent<Block>();
    }
    public void Move(int _newX, int _newY)
    {
        StartCoroutine(MoveCoroutine(_newX, _newY));
    }

    IEnumerator MoveCoroutine(int _newX, int _newY)
    {
        block.gameObject.name = block.Sprite.Type.ToString() + $" [{_newX},{_newY}]"; 
        block.X = _newX;
        block.Y = _newY;

        float time = 0;
        while (time <= 1)
        {
            yield return null;
            time += Time.deltaTime * 1 / moveAnimDuration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(_newX, _newY, 0), time);
        }
        block.DeactiveChosenUI();
    }
}
