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

        public DataBaseWendigoParticles particles;
        public DataBaseWendigoSounds sounds;

        public void Sign(string _text)
        {
            debugText.text = _text;
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