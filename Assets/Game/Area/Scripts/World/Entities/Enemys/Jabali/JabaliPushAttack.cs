using UnityEngine;
using System;
using System.Linq;

namespace Tools.StateMachine
{
    public class JabaliPushAttack : JabaliStates
    {
        float pushSpeed;
        Action DealDamage;
        Action PlayCombat;
        float maxSpeed;
        GameObject feedbackCharge;
        SoundPool pool;
        AudioSource source;
        CharacterGroundSensor groundSensor;

        float timer;
        float timePush;

        public JabaliPushAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _speed, Action _DealDamage,
                                GameObject _feedbackCharge, Action _PlayCombat, string _wooshSound, float _chargeDuration, CharacterGroundSensor _groundSensor) : base(myState, _sm)
        {
            maxSpeed = _speed;
            pushSpeed = maxSpeed / 1.5f;
            DealDamage = _DealDamage;
            feedbackCharge = _feedbackCharge;
            PlayCombat = _PlayCombat;
            pool = AudioManager.instance.GetSoundPool(_wooshSound);
            timePush = _chargeDuration;
            groundSensor = _groundSensor;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);
            feedbackCharge.SetActive(true);
            feedbackCharge.GetComponentsInChildren<ParticleSystem>()
                .ToList()
                .ForEach(x => x.Play());
            PlayCombat();
            anim.SetTrigger("ChargeOk");
            source = pool.Get();
            if (source != null)
            {
                source.transform.position = root.transform.position;
                source.Play();
            }
        }

        protected override void Update()
        {
            if (pushSpeed < maxSpeed)
            {
                pushSpeed += Time.deltaTime;

                if (pushSpeed > maxSpeed)
                    pushSpeed = maxSpeed;
            }

            rb.velocity = new Vector3(root.forward.x * pushSpeed, groundSensor.VelY, root.forward.z * pushSpeed);
            DealDamage();

            timer += Time.deltaTime;

            if (timer >= timePush)
                sm.SendInput(JabaliEnemy.JabaliInputs.IDLE);

            base.Update();
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            feedbackCharge.SetActive(false);
            feedbackCharge.GetComponentsInChildren<ParticleSystem>()
                .ToList()
                .ForEach(x => x.Stop());

            base.Exit(input);
            rb.velocity = Vector3.zero;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            if (source != null)
            {
                source.Stop();
                pool.ReturnToPool(source);
            }

            timer = 0;
        }
    }
}

