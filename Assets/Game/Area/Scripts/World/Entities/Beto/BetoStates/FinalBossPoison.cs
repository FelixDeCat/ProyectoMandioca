using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace IA2Final.FSM
{
    public class FinalBossPoison : MonoBaseState
    {
        public override event Action OnNeedsReplan;

        FinalPoisonLakeSkill skill;
        BetoBoss boss;
        bool timerComplete;
        Animator anim;
        List<AStarNode> path = new List<AStarNode>();
        AStarHelper aStar;
        bool canWalk;
        bool dest;
        int currentNode;
        AStarNode endNode;
        Vector3 fontPos;

        public FinalBossPoison(BetoBoss _boss, FinalPoisonLakeSkill _skill, Animator _anim, AStarHelper _aStar, Vector3 _fontPos)
        {
            boss = _boss;
            skill = _skill;
            anim = _anim;
            aStar = _aStar;
            fontPos = _fontPos;
            skill.LakeUp += EndSkill;
        }

        public override void UpdateLoop()
        {
            if (!canWalk || dest) return;

            if (boss.WalkToNextNode(path[currentNode]))
            {
                currentNode += 1;

                if (currentNode >= path.Count)
                {
                    dest = true;
                    boss.StartPoisonLake(skill);
                    boss.StopMove();
                    boss.LakeActive(true);
                    return;
                }
            }
        }

        void EndSkill()
        {
            timerComplete = true;
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            if(endNode == null)
                endNode = aStar.GetNearNode(fontPos);

            AStarNode startNode = aStar.GetNearNode(boss.transform.position);

            if(endNode == startNode)
            {
                boss.StartPoisonLake(skill);
                boss.LakeActive(true);
            }
            else
            {
                aStar.ExecuteAStar(startNode, endNode, OnGetPath);
                anim.Play("Move");
            }

            timerComplete = false;
        }

        void OnGetPath(IEnumerable<AStarNode> _path)
        {
            path = _path.ToList();
            canWalk = true;
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            currentNode = 0;
            dest = false;
            canWalk = false;
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

                if (!boss.SpawnCooldown)
                {
                    if (Transitions.ContainsKey(BetoStatesName.OnSpawn)) return Transitions[BetoStatesName.OnSpawn];
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
