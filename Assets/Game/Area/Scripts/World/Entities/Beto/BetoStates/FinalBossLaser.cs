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

                if (!boss.SpawnCooldown || (!boss.LakeCooldown && boss.Flying))
                {
                    if (!boss.SpawnCooldown && Transitions.ContainsKey(BetoStatesName.OnSpawn)) return Transitions[BetoStatesName.OnSpawn];
                    if (!boss.LakeCooldown && boss.Flying && Transitions.ContainsKey(BetoStatesName.OnPoisonLake)) return Transitions[BetoStatesName.OnPoisonLake];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                OnNeedsReplan?.Invoke();
            }

            return this;
        }
    }
}
