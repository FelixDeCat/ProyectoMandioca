using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossIdleState : MonoBaseState
    {
        public override event Action OnNeedsReplan;
        BossModel model;

        public BossIdleState(BossModel _model)
        {
            model = _model;
        }

        public override void UpdateLoop()
        {
            model.RotateToChar();
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {

            if (model.DistanceToCharacter())
            {
                if (!model.TPOnCooldown)
                {
                    if (Transitions.ContainsKey(GOAPStatesName.OnTPAbility)) return Transitions[GOAPStatesName.OnTPAbility];
                    OnNeedsReplan?.Invoke();
                    Debug.Log("se intenta tepear");
                    return this;
                }
            }
            if(!model.ShieldActive && model.CurrentStamina <= 0)
            {
                if (Transitions.ContainsKey(GOAPStatesName.OnStunAbility)) return Transitions[GOAPStatesName.OnStunAbility];

                OnNeedsReplan?.Invoke();
                return this;
            }
            else if (!model.AbilityOnCooldown && !model.ShieldActive)
            {
                if (Transitions.ContainsKey(GOAPStatesName.OnFlameAbility)) return Transitions[GOAPStatesName.OnFlameAbility];
                else if (Transitions.ContainsKey(GOAPStatesName.OnShootAbility)) return Transitions[GOAPStatesName.OnShootAbility];
                else if (Transitions.ContainsKey(GOAPStatesName.OnSpawnAbility)) return Transitions[GOAPStatesName.OnSpawnAbility];
                OnNeedsReplan?.Invoke();
                return this;
            }
            else if (!model.AttackOnCooldown)
            {
                if (!model.DistanceToCharacter() && Transitions.ContainsKey(GOAPStatesName.OnShootAttack)) return Transitions[GOAPStatesName.OnShootAttack];
                else if (model.DistanceToCharacter() && Transitions.ContainsKey(GOAPStatesName.OnMeleAttack)) return Transitions[GOAPStatesName.OnMeleAttack];
                Debug.Log("quiere atacar");
                OnNeedsReplan?.Invoke();
                return this;
            }

            return this;
        }
    }
}
