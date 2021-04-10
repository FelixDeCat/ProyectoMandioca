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

            if(boss.WalkToNextNode(path[currentNode]))
            {
                currentNode += 1;

                if(currentNode >= path.Count)
                {
                    dest = true;
                    anim.Play("Idle");
                    return;
                }
            }
        }

        public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
        {
            AStarNode startNode = aStar.GetNearNode(boss.transform.position);

            AStarNode endNode = aStar.GetRandomNode(aStar.GetNearNodes(target.position, charDistance));

            aStar.ExecuteAStar(startNode, endNode, OnGetPath);
            anim.Play("Move");
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
            return this;
        }
    }
}
