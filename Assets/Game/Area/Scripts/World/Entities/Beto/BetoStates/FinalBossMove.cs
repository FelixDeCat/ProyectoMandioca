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
        Transform root;
        Animator anim;
        BetoBoss boss;
        List<AStarNode> path = new List<AStarNode>();
        float speed;
        Rigidbody rb;
        AStarHelper aStar;
        bool canWalk;
        bool dest;
        float charDistance;
        int currentNode;
        float rotSpeed;

        public FinalBossMove(BetoBoss _boss, Animator _anim, AStarHelper _aStar, Transform _target, Transform _root,
            float _speed, float _rotSpeed,float _charDistance, Rigidbody _rb)
        {
            boss = _boss;
            target = _target;
            rb = _rb;
            speed = _speed;
            anim = _anim;
            aStar = _aStar;
            charDistance = _charDistance;
            rotSpeed = _rotSpeed;
            root = _root;
        }

        public override void UpdateLoop()
        {
            if (!canWalk || dest) return;

            if(Vector3.Distance(boss.transform.position, path[currentNode].transform.position) < 0.1f)
            {
                currentNode += 1;

                if(currentNode >= path.Count)
                {
                    dest = true;
                    return;
                }
            }

            Vector3 dirToNode = (path[currentNode].transform.position - boss.transform.position).normalized;

            root.forward = Vector3.Lerp(root.forward, dirToNode, Time.deltaTime * rotSpeed);

            rb.velocity = new Vector3(root.forward.x * speed, rb.velocity.y, root.forward.z * speed);
        }

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
        }

        public override Dictionary<string, object> Exit(IState to)
        {
            return base.Exit(to);
        }

        public override IState ProcessInput()
        {
            return this;
        }
    }
}
