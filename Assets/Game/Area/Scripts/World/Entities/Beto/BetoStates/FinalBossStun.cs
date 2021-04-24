using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossStun : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        Rigidbody rb;

        float timeStuned;
        bool stuned;
        float timer;
        bool timerComplete;
        BossSkills expansiveSkill;
        BetoBoss boss;


        public FinalBossStun(BetoBoss _boss, BossSkills _expansiveSkill, float _timeStuned, Rigidbody _rb)
        {
            boss = _boss;
            expansiveSkill = _expansiveSkill;
            timeStuned = _timeStuned;
            rb = _rb;
        }

        public override void UpdateLoop()
        {
            if (timerComplete) return;

            if (stuned)
            {
                timer += Time.deltaTime;

                if(timer >= timeStuned)
                {
                    expansiveSkill.UseSkill(EndSkill);
                    boss.Stuned = false;
                    stuned = false;
                }
            }
            else
            {
                expansiveSkill.OnUpdate();
            }
        }

        void EndSkill()
        {
            timerComplete = true;
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = true;
            stuned = true;
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            stuned = false;
            boss.Stuned = false;
            timerComplete = false;
            timer = 0;
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (!timerComplete) return this;

            OnNeedsReplan?.Invoke();
            return this;
        }
    }
}
