using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystem.Particle[] particles;
    int numParticlesAlive;
    bool soundPlayed;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void LateUpdate()
    {
        if (particles == null || particles.Length < particle.main.maxParticles)
            particles = new ParticleSystem.Particle[particle.main.maxParticles];

        numParticlesAlive = particle.GetParticles(particles);
        if (!soundPlayed && numParticlesAlive > 0)
        {
            AudioManager.Instance.Play("fireworks_" + Random.Range(0, 7));
            soundPlayed = true;
        }
        if(numParticlesAlive == 0)
        {
            soundPlayed = false;
        }
    }
}
