using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossShootAbility : MonoBaseState
    {
        BossSkills phantomSkill;
        public override event Action OnNeedsReplan;
        BossModel model;
        bool timerComplete;

        public BossShootAbility(BossModel _model, BossSkills _skill)
        {
            model = _model;
            phantomSkill = _skill;
        }

        public override void UpdateLoop()
        {
            phantomSkill.OnUpdate();
            Debug.Log("me quedo acá eternamente");
        }

        void CompleteAbility()
        {
            timerComplete = true;
            model.AbilityCooldown();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            phantomSkill.UseSkill(CompleteAbility);
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
                    Debug.Log("me quiero tepear");
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (Transitions.ContainsKey(GOAPStatesName.OnIdle)) return Transitions[GOAPStatesName.OnIdle];

                if (!model.AttackOnCooldown)
                {
                    Debug.Log("efectivamente");
                    if (Transitions.ContainsKey(GOAPStatesName.OnMeleAttack) && model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnMeleAttack];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnShootAttack) && !model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnShootAttack];
                    Debug.Log(Transitions.ContainsKey(GOAPStatesName.OnShootAttack));
                }
            }

            return this;
        }
    }
}
