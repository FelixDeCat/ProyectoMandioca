using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM
{
    public class BossShootState : MonoBaseState
    {

        public override event Action OnNeedsReplan;
        BossModel model;
        Animator anim;
        float timer;
        float animDuration = 2;
        bool timerComplete;

        public BossShootState(BossModel _model, Animator _anim)
        {
            anim = _anim;
            model = _model;
        }

        public override void UpdateLoop()
        {
            if (!timerComplete)
            {
                timer += Time.deltaTime;
                if (timer >= animDuration)
                {
                    model.AttackCooldown();
                    timerComplete = true;
                }
            }
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            base.Enter(from, transitionParameters);
            anim.Play("NormalShot");
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            timer = 0;
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