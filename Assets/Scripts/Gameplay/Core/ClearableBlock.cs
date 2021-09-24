using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for playing clear animation and sound effects.
/// </summary>
public class ClearableBlock : MonoBehaviour
{
    [Header("--- Animation ---")]
    [SerializeField] AnimationClip clearAnim; // The clear animation, make sprite smaller and disappear.
    [SerializeField] Animator animator; //Animator.

    [Header("--- VFX ---")]
    [SerializeField] GameObject clearEffect; //The particle effect played when a block is cleared.

    Block block; // The block component
    BlockVisualFX blockEffect;

    void Awake()
    {
        block = GetComponent<Block>();
        blockEffect = GetComponent<BlockVisualFX>();
    }

    /// <summary>
    /// Play clear animation and clear sound effect.
    /// </summary>
    public void Clear()
    {
        blockEffect.PlayClearEffect();
        StartCoroutine(ClearAnimationCoroutine());
    }

    /// <summary>
    /// Play clear animation.
    /// </summary>
    IEnumerator ClearAnimationCoroutine()
    {
        animator.Play(clearAnim.name);
        yield return new WaitForSeconds(clearAnim.length / 2F);
        Destroy(block.gameObject);
        block.Grid.SpawnNewBlock(block.X, block.Y, BlockState.EMPTY);
    }

}
