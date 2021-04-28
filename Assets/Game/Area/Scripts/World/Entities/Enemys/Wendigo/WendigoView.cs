using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Tools.StateMachine
{

    public class WendigoView : MonoBehaviour    //Ponele MonoB?
    {
        [SerializeField]
        TMP_Text debugText = null;
        [SerializeField]
        TMP_Text distanceText = null;

        [SerializeField] Animator anim = null;
        [SerializeField] GameObject handRock = null;

        public DataBaseWendigoParticles particles;
        public DataBaseWendigoSounds sounds;

        public void Awakening()
        {
            sounds.mytransform = transform;
            anim.SetTrigger("Awake");
            sounds.Initianlize();
            particles.Initialize();
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
            sounds.WalkClip();
        }
        public void ExitMov()
        {
            anim.SetFloat("velocity", 0);
        }
        public void Kick()
        {
            anim.SetTrigger("Kick");
            sounds.HitTheGround();
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
            sounds.ThrowAttackClip();
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
            sounds.GetDamageClip();
            ParticlesManager.Instance.PlayParticle(particles.takeDmg.name, transform.position);
        }
        public void Death()
        {
            sounds.DeathClip();
        }
    }

    [System.Serializable]
    public class DataBaseWendigoParticles
    {
        public ParticleSystem castParticles = null;
        public ParticleSystem takeDmg = null;

        public void Initialize()
        {
            ParticlesManager.Instance.GetParticlePool(takeDmg.name, takeDmg, 5);
        }
    }

    [System.Serializable]
    public class DataBaseWendigoSounds
    {
        public Transform mytransform;
        [SerializeField] AudioClip _getDamageClip = null;
        [SerializeField] AudioClip _throwAttackClip = null;
        [SerializeField] AudioClip _beginFightClip = null;
        [SerializeField] AudioClip _deathClip = null;
        [SerializeField] AudioClip _hitTheGround = null;
        [SerializeField] AudioClip _walk = null;

        public void Initianlize()
        {
            AudioManager.instance.GetSoundPool(_getDamageClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _getDamageClip);
            AudioManager.instance.GetSoundPool(_throwAttackClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _throwAttackClip);
            AudioManager.instance.GetSoundPool(_beginFightClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _beginFightClip);
            AudioManager.instance.GetSoundPool(_deathClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _deathClip);
            AudioManager.instance.GetSoundPool(_hitTheGround.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _hitTheGround);
            AudioManager.instance.GetSoundPool(_walk.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _walk);
        }
        public void SetRoot(Transform root) => mytransform = root;
        public void GetDamageClip() => AudioManager.instance.PlaySound(_getDamageClip.name, mytransform);
        public void ThrowAttackClip() => AudioManager.instance.PlaySound(_throwAttackClip.name, mytransform);
        public void BeginFightClip() => AudioManager.instance.PlaySound(_beginFightClip.name, mytransform);
        public void HitTheGround() => AudioManager.instance.PlaySound(_hitTheGround.name, mytransform);
        public void DeathClip() => AudioManager.instance.PlaySound(_deathClip.name, mytransform);
        public void WalkClip() => AudioManager.instance.PlaySound(_walk.name, mytransform);
    }

}