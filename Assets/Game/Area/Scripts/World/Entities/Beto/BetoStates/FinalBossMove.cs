using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace IA2Final.FSM
{
    public class FinalBossMove : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        Transform target;
        Animator anim;
        BetoBoss boss;
        List<AStarNode> path = new List<AStarNode>();
        AStarHelper aStar;
        bool canWalk;
        bool dest;
        float charDistance;
        int currentNode;

        public FinalBossMove(BetoBoss _boss, Animator _anim, AStarHelper _aStar, Transform _target, float _charDistance)
        {
            boss = _boss;
            target = _target;
            anim = _anim;
            aStar = _aStar;
            charDistance = _charDistance;
        }

        public override void UpdateLoop()
        {
            if (!canWalk || dest) return;
            if(currentNode >= path.Count)
            {
                dest = true;
                anim.Play("Idle");
                return;
            }

            if(boss.WalkToNextNode(path[currentNode]))
            {
                currentNode += 1;
                timer = 0;
                if(currentNode >= path.Count)
                {
                    dest = true;
                    anim.Play("Idle");
                    return;
                }
            }
            else
            {
                timer += Time.deltaTime;

                if(timer >= maxTimeToNode)
                {
                    dest = true;
                    anim.Play("Idle");
                    return;
                }
            }
        }
        float timer;
        float maxTimeToNode = 10;

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            AStarNode startNode = aStar.GetNearNode(boss.transform.position);

            AStarNode endNode = aStar.GetRandomNode(aStar.GetNearNodes(target.position, charDistance));

            aStar.ExecuteAStar(startNode, endNode, OnGetPath);
        }

        void OnGetPath(IEnumerable<AStarNode> _path)
        {
            path = _path.ToList();
            canWalk = true;
            anim.Play("Move");
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            currentNode = 0;
            timer = 0;
            dest = false;
            canWalk = false;
            anim.Play("Idle");
            boss.StopMove();
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            if (boss.Stuned)
                return Transitions[BetoStatesName.OnStun];

            if (!dest) return this;

            if (Transitions.Count != 0)
            {
                if (!boss.Flying && !boss.FlyCooldown)
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnFly)) return Transitions[BetoStatesName.OnFly];
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

                OnNeedsReplan?.Invoke();
            }

            return this;
        }
    }
}
