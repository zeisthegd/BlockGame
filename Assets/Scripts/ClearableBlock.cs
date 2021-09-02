using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearableBlock : MonoBehaviour
{
    [Header("--- Animation ---")]
    [SerializeField] AnimationClip clearAnim;
    [SerializeField] Animator animator;

    [Header("--- VFX ---")]
    [SerializeField] GameObject clearEffect;

    Block block;

    void Awake()
    {
        block = GetComponent<Block>();
    }
    public void Clear()
    {
        StartCoroutine(ClearAnimationCoroutine());
        StartCoroutine(PlayClearEffect());
    }

    IEnumerator ClearAnimationCoroutine()
    {
        animator.Play(clearAnim.name);
        yield return new WaitForSeconds(clearAnim.length);
    }

    IEnumerator PlayClearEffect()
    {
        List<ParticleSystem> clearEffect = GetClearParticles();
        foreach (ParticleSystem particle in clearEffect)
        {
            particle.Play();
        }

        yield return new WaitForSeconds(.3F);
        Destroy(block.gameObject);
        block.Grid.SpawnNewBlock(block.X, block.Y, BlockMode.EMPTY);
    }

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
