using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossTPState : MonoBaseState
    {
        BossSkills tpSkill;
        public override event Action OnNeedsReplan;
        BossModel model;
        bool timerComplete;

        public BossTPState(BossModel _model, BossSkills _skill)
        {
            model = _model;
            tpSkill = _skill;
        }

        public override void UpdateLoop()
        {
            tpSkill.OnUpdate();
        }

        void CompleteAbility()
        {
            timerComplete = true;
            model.TPCooldown();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            tpSkill.UseSkill(CompleteAbility);
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
                if (Transitions.ContainsKey(GOAPStatesName.OnIdle)) return Transitions[GOAPStatesName.OnIdle];

                if (!model.AttackOnCooldown)
                {
                    if (Transitions.ContainsKey(GOAPStatesName.OnMeleAttack) && model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnMeleAttack];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnShootAbility) && !model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnShootAbility];
                }

                if (!model.AbilityOnCooldown)
                {
                    if (Transitions.ContainsKey(GOAPStatesName.OnFlameAbility)) return Transitions[GOAPStatesName.OnFlameAbility];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnShootAbility)) return Transitions[GOAPStatesName.OnShootAbility];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnSpawnAbility)) return Transitions[GOAPStatesName.OnSpawnAbility];
                }
            }

            OnNeedsReplan?.Invoke();
            return this;
        }
    }
}
