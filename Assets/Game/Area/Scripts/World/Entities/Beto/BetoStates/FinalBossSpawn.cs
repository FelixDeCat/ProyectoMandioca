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
            timerComplete = false;
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
            if (boss.Stuned)
                return Transitions[BetoStatesName.OnStun];

            if (!timerComplete) return this;

            if (Transitions.Count != 0)
            {
                if (!boss.Flying && !boss.FlyCooldown)
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnFly)) return Transitions[BetoStatesName.OnFly];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (boss.DistanceToCharacter())
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnMove)) return Transitions[BetoStatesName.OnMove];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (Transitions.ContainsKey(BetoStatesName.OnIdle)) return Transitions[BetoStatesName.OnIdle];

                if (!boss.LakeCooldown && boss.Flying)
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnPoisonLake)) return Transitions[BetoStatesName.OnPoisonLake];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (!boss.AttackOnCooldown)
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnShoot)) return Transitions[BetoStatesName.OnShoot];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                OnNeedsReplan?.Invoke();
            }

            return this;
        }
    }
}
