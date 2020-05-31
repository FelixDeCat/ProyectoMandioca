using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class JabaliCharge : JabaliStates
    {
        float chargeTime;
        float timer = 0;
        Vector3 finalPos;
        string firstPushSound;
        SoundPool pool;
        AudioSource source;

        public JabaliCharge(EState<JabaliEnemy.JabaliInputs> myState, EventStateMachine<JabaliEnemy.JabaliInputs> _sm, float _chargeTime,
            string _updateSound, string _exitSound) : base(myState, _sm)
        {
            chargeTime = _chargeTime;
            firstPushSound = _exitSound;
            pool = AudioManager.instance.GetSoundPool(_updateSound);
        }

        protected override void Enter(EState<JabaliEnemy.JabaliInputs> input)
        {
            if (input.Name != "Petrified")
            {
                anim.SetBool("ChargeAttack", true);
            }
            finalPos = root.position - root.forward * 2;
            source = pool.Get();
            source.Play();
        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            rb.transform.position = Vector3.Lerp(root.position, finalPos, Time.deltaTime);

            if (timer >= chargeTime)
                sm.SendInput(JabaliEnemy.JabaliInputs.PUSH);
        }

        protected override void Exit(JabaliEnemy.JabaliInputs input)
        {
            if (input != JabaliEnemy.JabaliInputs.PETRIFIED && input != JabaliEnemy.JabaliInputs.DEAD)
            {
                anim.SetBool("ChargeAttack", false);
                timer = 0;
            }
            source.Stop();
            pool.ReturnToPool(source);
            AudioManager.instance.PlaySound(firstPushSound);
        }
    }
}
