using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IA2Final.FSM
{
    public class FinalBossFly : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        Animator anim;
        Rigidbody rb;
        BetoBoss boss;
        bool flyOver;

        public FinalBossFly(BetoBoss _boss, Animator _anim, Rigidbody _rb)
        {
            boss = _boss;
            anim = _anim;
            rb = _rb;
        }

        public override void UpdateLoop()
        {
            if (flyOver) return;
            flyOver = boss.Fly();
            if(flyOver) rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            flyOver = false;
            rb.useGravity = false;
            anim.SetBool("StartFly", true);
            anim.Play("StartFly");
            boss.StartFly();
            boss.Flying = true;
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            boss.EndFly();
            flyOver = false;
            anim.SetBool("StartFly", false);
            anim.SetFloat("Flying", 1);
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (boss.Stuned)
                return Transitions[BetoStatesName.OnStun];

            if (!flyOver) return this;

            if (Transitions.Count != 0)
            {
                if (boss.DistanceToCharacter())
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnMove)) return Transitions[BetoStatesName.OnMove];
                    OnNeedsReplan?.Invoke();
                    return this;
                }

                if (Transitions.ContainsKey(BetoStatesName.OnIdle)) return Transitions[BetoStatesName.OnIdle];

                if (!boss.SpawnCooldown || !boss.LakeCooldown)
                {
                    if (!boss.SpawnCooldown && Transitions.ContainsKey(BetoStatesName.OnSpawn)) return Transitions[BetoStatesName.OnSpawn];
                    if (!boss.LakeCooldown && Transitions.ContainsKey(BetoStatesName.OnPoisonLake)) return Transitions[BetoStatesName.OnPoisonLake];

                    foreach (var item in Transitions)
                    {
                        Debug.Log(item.Key);
                    }
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
