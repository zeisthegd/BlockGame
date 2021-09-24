using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    public GameObject clearEffect;
    public GameObject specialEffect;

    Block block;

    void Start()
    {
        block = GetComponent<Block>();
    }

    /// <summary>
    /// Play clear particle effect.
    /// </summary>
    public void PlayClearEffect()
    {
        StartCoroutine(PlayEffects(clearEffect));
    }

    /// <summary>
    /// Play particle effect of special block.
    /// </summary>
    public void PlaySpecialEffect()
    {
        StartCoroutine(PlayEffects(specialEffect));
    }

    /// <summary>
    /// Get particle effects from a prefab an play them.
    /// </summary>
    IEnumerator PlayEffects(GameObject effectPrefabs)
    {
        List<ParticleSystem> effectSystem = GetEffects(effectPrefabs);
        foreach (ParticleSystem effect in effectSystem)
        {
            effect.Play();
        }
        yield return new WaitForSeconds(1F);
    }

    /// <summary>
    /// Get the particle effect from a prefabs.
    /// </summary>
    /// <returns>List of particle effects</returns>
    List<ParticleSystem> GetEffects(GameObject effectPrefab)
    {
        List<ParticleSystem> particles = new List<ParticleSystem>();
        var effect = Instantiate(effectPrefab, transform.position, Quaternion.identity, this.transform);
        for (int i = 0; i < effectPrefab.transform.childCount; i++)
        {
            if (effectPrefab.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>())
                particles.Add(effectPrefab.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>());
        }
        return particles;
    }
}
