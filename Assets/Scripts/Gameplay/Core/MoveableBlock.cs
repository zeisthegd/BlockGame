using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for moving the blocks
/// </summary>
public class MoveableBlock : MonoBehaviour
{
    [Header("--- Animation ---")]
    [SerializeField] float moveAnimDuration; // Move animation duration
    Block block; // The block component to modify its info: position, name


    void Awake()
    {
        block = GetComponent<Block>();
    }

    /// <summary>
    /// Start the move coroutine.
    /// </summary>
    /// <param name="newX">X position to move to</param>
    /// <param name="newY">Y position to move to</param>
    public void Move(int newX, int newY)
    {
        StartCoroutine(MoveCoroutine(newX, newY));
    }

    /// <summary>
    /// Move its position in 2D space.
    /// </summary>
    /// <param name="newX">New X</param>
    /// <param name="newY">New Y</param>
    /// <returns></returns>
    IEnumerator MoveCoroutine(int newX, int newY)
    {
        block.DeactiveChosenUI();
        block.gameObject.name = block.Data.Type.ToString() + $" [{newX},{newY}]";
        block.X = newX;
        block.Y = newY;

        float time = 0;
        while (time <= 1)
        {
            yield return null;
            time += (Time.deltaTime * 1 / moveAnimDuration);
            transform.position = Vector3.Lerp(transform.position, new Vector3(newX, newY, 0), time);
        }

    }
}
