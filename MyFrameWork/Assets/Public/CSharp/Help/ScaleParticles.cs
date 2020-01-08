using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleParticles : MonoBehaviour
{
    public float ScaleSize = 1.0f;
    private List<float> initialSizes = new List<float>();

    void Awake()
    {
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            initialSizes.Add(particle.startSize);
            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer)
            {
                initialSizes.Add(renderer.lengthScale);
                initialSizes.Add(renderer.velocityScale);
            }
        }
    }

    void Update()
    {
        gameObject.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        for(int i=0;i<particles.Length;i++)
        {
            particles[i].startSize = initialSizes[i] * gameObject.transform.localScale.magnitude;
            ParticleSystemRenderer renderer = particles[i].GetComponent<ParticleSystemRenderer>();
            if (renderer)
            {
                renderer.lengthScale = initialSizes[i] * gameObject.transform.localScale.magnitude;
                renderer.velocityScale = initialSizes[i] * gameObject.transform.localScale.magnitude;
            }
        }
    }

}