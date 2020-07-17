using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharParticles
{
    public ParticleSystem evadeParticle = null;
    public ParticleSystem HeavyLoaded = null;
    public ParticleSystem feedbackHeavy = null;
    public ParticleSystem feedbackDashHeavy = null;
    public ParticleSystem slash_right = null;
    public ParticleSystem slash_left = null;
    public ParticleSystem parryParticle = null;
    public ParticleSystem blockParticle = null;
    public ParticleSystem hitParticle = null;

    public void Initialize()
    {

    }
}
