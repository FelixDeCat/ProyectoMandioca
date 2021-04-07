using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossPoison : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        FinalPoisonLakeSkill skill;
        BetoBoss boss;
        bool timerComplete;

        public FinalBossPoison(BetoBoss _boss, FinalPoisonLakeSkill _skill)
        {
            boss = _boss;
            skill = _skill;
            skill.LakeUp += EndSkill;
        }

        public override void UpdateLoop()
        {
             
        }

        void EndSkill()
        {
            timerComplete = true;
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            boss.StartPoisonLake(skill);
            boss.AbilityCooldown();
            timerComplete = false;
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
