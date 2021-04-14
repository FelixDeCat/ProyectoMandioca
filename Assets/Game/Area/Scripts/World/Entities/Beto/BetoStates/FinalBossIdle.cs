using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossIdle : MonoBaseState
    {
        public override event Action OnNeedsReplan;
        BetoBoss boss;

        public FinalBossIdle(BetoBoss _boss)
        {
            boss = _boss;
        }

        public override void UpdateLoop()
        {
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (boss.Stuned)
                return Transitions[BetoStatesName.OnStun];

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

            if (!boss.SpawnCooldown || (!boss.LakeCooldown && boss.Flying))
            {
                if (!boss.SpawnCooldown && Transitions.ContainsKey(BetoStatesName.OnSpawn)) return Transitions[BetoStatesName.OnSpawn];
                if (!boss.LakeCooldown && boss.Flying && Transitions.ContainsKey(BetoStatesName.OnPoisonLake)) return Transitions[BetoStatesName.OnPoisonLake];
                OnNeedsReplan?.Invoke();
                return this;
            }
            if (!boss.AttackOnCooldown)
            {
                if (Transitions.ContainsKey(BetoStatesName.OnShoot)) return Transitions[BetoStatesName.OnShoot];
                OnNeedsReplan?.Invoke();
                return this;
            }

            return this;
        }
    }
}
