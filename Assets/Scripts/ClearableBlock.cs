using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearableBlock : MonoBehaviour
{
    [SerializeField] AnimationClip clearAnim;
    [SerializeField] Animator animator;
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
        block.Grid.SpawnNewBlock(block.X, block.Y, BlockMode.EMPTY);
        
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
        yield return new WaitForSeconds(0.3F);
        Destroy(this.gameObject);
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
