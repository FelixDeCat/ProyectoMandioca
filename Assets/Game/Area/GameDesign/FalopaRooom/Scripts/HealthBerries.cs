using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBerries : MonoBehaviour
{
    [SerializeField] int healthAmount;

    [SerializeField] ParticleSystem feedback;

   public void ConsumeBerries()
    {
        if (feedback != null) feedback.Play();

        Main.instance.GetChar().Life.Heal(healthAmount);
    }
}
