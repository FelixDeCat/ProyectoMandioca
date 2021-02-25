using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossSpawnState : MonoBaseState
    {
        SpawnSkill spawnSkill;
        public override event Action OnNeedsReplan;
        BossModel model;
        bool timerComplete;

        public BossSpawnState(BossModel _model, SpawnSkill _skill)
        {
            model = _model;
            spawnSkill = _skill;
        }

        public override void UpdateLoop()
        {
        }

        void CompleteAbility()
        {
            model.ShieldActive = true;
            timerComplete = true;
            model.AbilityCooldown();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            spawnSkill.UseSkill(() => model.ShieldActive = false);
            spawnSkill.OnSpawn = CompleteAbility;
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            timerComplete = false;
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (!timerComplete) return this;

            if (Transitions.Count != 0)
            {
                if (!model.TPOnCooldown && model.DistanceToCharacter())
                {
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (Transitions.ContainsKey(GOAPStatesName.OnIdle)) return Transitions[GOAPStatesName.OnIdle];

                if (!model.AttackOnCooldown)
                {
                    if (Transitions.ContainsKey(GOAPStatesName.OnMeleAttack) && model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnMeleAttack];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnShootAbility) && !model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnShootAbility];
                }
            }

            OnNeedsReplan?.Invoke();
            return this;
        }
    }
}
