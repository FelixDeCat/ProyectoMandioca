using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Tools.StateMachine
{

    public class WendigoView : MonoBehaviour    //Ponele MonoB?
    {
        [SerializeField]
        TMP_Text debugText;
        [SerializeField]
        TMP_Text distanceText;

        [SerializeField] Animator anim;
        [SerializeField] GameObject handRock;

        public DataBaseWendigoParticles particles;
        public DataBaseWendigoSounds sounds;

        public void Awakening()
        {
            anim.SetTrigger("Awake");
        }
        public void DebugText(string _state)
        {
            debugText.text = _state;
        }
        public void DistanceText(string _dist)
        {
            distanceText.text = _dist;
        }
        //Funciones con particulas, sonidos y animacion
        public void Movement(float vel)
        {
            anim.SetFloat("velocity", vel);
        }
        public void ExitMov()
        {
            anim.SetFloat("velocity", 0);
        }
        public void Kick()
        {
            anim.SetTrigger("Kick");
        }
        public void GrabThing()
        {
            anim.SetTrigger("GrabThing");
        }
        public void Reset()
        {
            handRock.SetActive(false);
        }
        public void Throw()
        {
            anim.SetTrigger("Throw");

        }
        public void TurnOffThing()
        {
            handRock.SetActive(false);
        }
        public void TurnOnThing()
        {

            handRock.SetActive(true);
        }
        public void Damaged()
        {

        }
    }
    public class DataBaseWendigoParticles
    {
        public ParticleSystem castParticles = null;
        public ParticleSystem takeDmg = null;
    }

    [System.Serializable]
    public class DataBaseWendigoSounds
    {
        public AudioClip takeDmgSound;
        public AudioClip attackSound;
    }

}