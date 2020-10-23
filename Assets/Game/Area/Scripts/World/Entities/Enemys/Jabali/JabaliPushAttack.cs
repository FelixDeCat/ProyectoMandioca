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
        float timePush;
        bool isGoat;

        public JabaliPushAttack(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _speed, Action _DealDamage,
                                GameObject _feedbackCharge, Action _PlayCombat, string _wooshSound, float _chargeDuration, CharacterGroundSensor _groundSensor, bool _isGoat = false) : base(myState, _sm)
        {
            maxSpeed = _speed;
            pushSpeed = maxSpeed / 1.5f;
            DealDamage = _DealDamage;
            feedbackCharge = _feedbackCharge;
            PlayCombat = _PlayCombat;
            pool = AudioManager.instance.GetSoundPool(_wooshSound);
            timePush = _chargeDuration;
            groundSensor = _groundSensor;
            isGoat = _isGoat;
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            base.Enter(input);
            feedbackCharge.SetActive(true);
            feedbackCharge.GetComponentsInChildren<ParticleSystem>()
                .ToList()
                .ForEach(x => x.Play());

            if (isGoat) anim.SetBool("ToIdle", false);
            PlayCombat();
            anim.SetTrigger("ChargeOk");
            source = pool.Get();
            if (source != null)
            {
                source.transform.position = root.transform.position;
                source.Play();
            }

            cdModule.AddCD("PushDuration", () => sm.SendInput(JabaliEnemy.JabaliInputs.IDLE), timePush);
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

            base.Update();
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            feedbackCharge.SetActive(false);
            feedbackCharge.GetComponentsInChildren<ParticleSystem>()
                .ToList()
                .ForEach(x => x.Stop());

            if (isGoat && JabaliEnemy.JabaliInputs.IDLE == input) anim.SetBool("ToIdle", true);

            base.Exit(input);
            rb.velocity = Vector3.zero;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            if (source != null)
            {
                source.Stop();
                pool.ReturnToPool(source);
            }
            cdModule.EndCDWithoutExecute("PushDuration");
        }
    }
}

