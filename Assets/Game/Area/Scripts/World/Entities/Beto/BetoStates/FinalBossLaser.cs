using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossLaser : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        BossSkills skill;
        BetoBoss boss;
        bool timerComplete;

        public FinalBossLaser(BetoBoss _boss, BossSkills _skill)
        {
            boss = _boss;
            skill = _skill;
        }

        public override void UpdateLoop()
        {
            skill.OnUpdate();
            boss.RotateToChar();
        }

        void EndSkill()
        {
            timerComplete = true;
            boss.AttackCooldown();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            timerComplete = false;
            skill.UseSkill(EndSkill);
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
