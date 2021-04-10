using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossSpawn : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        BetoBoss boss;
        FinalThunderWaveSkill spawnSkill;
        bool timerComplete;

        public FinalBossSpawn(BetoBoss _boss, FinalThunderWaveSkill _skill)
        {
            boss = _boss;
            spawnSkill = _skill;
            spawnSkill.SpawnOver += EndSkill;
        }

        public override void UpdateLoop()
        {
            if (timerComplete) return;
            spawnSkill.OnUpdate();
        }

        void EndSkill()
        {
            timerComplete = true;
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            spawnSkill.UseSkill(() => boss.SpawnActive(false));
            boss.SpawnActive(true);
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            timerComplete = false;
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
