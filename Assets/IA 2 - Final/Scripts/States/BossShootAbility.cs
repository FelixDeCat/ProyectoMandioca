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
        int staminaNeed;

        public void ChangeSkill(BossSkills _skill) => phantomSkill = _skill;

        public BossShootAbility(BossModel _model, BossSkills _skill, int _staminaNeed)
        {
            staminaNeed = _staminaNeed;
            model = _model;
            phantomSkill = _skill;
        }


        public override void UpdateLoop()
        {
            phantomSkill.OnUpdate();
            model.RotateToChar();
        }

        void CompleteAbility()
        {
            timerComplete = true;
            model.ChangeLastAbility("Phantom");
            model.AbilityCooldown(staminaNeed);
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
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (Transitions.ContainsKey(GOAPStatesName.OnIdle)) return Transitions[GOAPStatesName.OnIdle];

                if (!model.AttackOnCooldown)
                {
                    if (Transitions.ContainsKey(GOAPStatesName.OnMeleAttack) && model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnMeleAttack];
                    else if (Transitions.ContainsKey(GOAPStatesName.OnShootAttack) && !model.DistanceToCharacter()) return Transitions[GOAPStatesName.OnShootAttack];
                }
            }

            OnNeedsReplan?.Invoke();

            return this;
        }
    }
}
