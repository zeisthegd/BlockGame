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
    
    void Awake()
    {
        block = GetComponent<Block>();
    }

    /// <summary>
    /// Play clear animation and clear sound effect.
    /// </summary>
    public void Clear()
    {
        StartCoroutine(ClearAnimationCoroutine());
        StartCoroutine(PlayClearEffect());
    }

    /// <summary>
    /// Play clear animation.
    /// </summary>
    IEnumerator ClearAnimationCoroutine()
    {
        animator.Play(clearAnim.name);
        yield return new WaitForSeconds(clearAnim.length);
    }

    /// <summary>
    /// Play clear particle effect.
    /// </summary>
    IEnumerator PlayClearEffect()
    {
        List<ParticleSystem> clearEffect = GetClearParticles();
        foreach (ParticleSystem particle in clearEffect)
        {
            particle.Play();
        }

        yield return new WaitForSeconds(.3F);
        Destroy(block.gameObject);
        block.Grid.SpawnNewBlock(block.X, block.Y, BlockState.EMPTY);
    }

    /// <summary>
    /// Get the particle effect from a prefabs.
    /// </summary>
    /// <returns>List of particle effects</returns>
    List<ParticleSystem> GetClearParticles()
    {
        List<ParticleSystem> particles = new List<ParticleSystem>();
        var _clearEffect = Instantiate(clearEffect, transform.position, Quaternion.identity, this.transform);
        for (int i = 0; i < clearEffect.transform.childCount; i++)
        {
            if (clearEffect.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>())
                particles.Add(clearEffect.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>());
        }
        return particles;
    }
}
