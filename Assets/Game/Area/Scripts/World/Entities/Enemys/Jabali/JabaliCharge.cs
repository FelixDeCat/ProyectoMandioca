using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class JabaliCharge : JabaliStates
    {
        float chargeTime;
        Vector3 finalPos;
        string firstPushSound;
        SoundPool pool;
        AudioSource source;
        GenericEnemyMove move;

        public JabaliCharge(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _chargeTime,
            string _updateSound, string _exitSound, GenericEnemyMove _move) : base(myState, _sm)
        {
            chargeTime = _chargeTime;
            firstPushSound = _exitSound;
            move = _move;
            pool = AudioManager.instance.GetSoundPool(_updateSound, AudioManager.SoundDimesion.ThreeD);
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            anim.SetBool("ChargeAttack", true);
            finalPos = root.position - root.forward * 2;
            source = pool.Get();
            if (source != null)
            {
                source.transform.position = root.transform.position;
                source.Play();
            }

            cdModule.AddCD("ChargeCD", () => sm.SendInput(JabaliEnemy.JabaliInputs.PUSH), chargeTime);
        }

        protected override void Update()
        {
            rb.transform.position = Vector3.Lerp(root.position, finalPos, Time.deltaTime);

            if (enemy.CurrentTarget())
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                myForward.y = 0;
                move.Rotation(myForward);
            }
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.DEAD)
            {
                anim.SetBool("ChargeAttack", false);
                AudioManager.instance.PlaySound(firstPushSound, root);
            }
            if (source != null)
            {
                source.Stop();
                pool.ReturnToPool(source);
            }
            cdModule.EndCDWithoutExecute("ChargeCD");
        }
    }
}
