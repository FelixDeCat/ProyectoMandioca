using UnityEngine;
using System;

namespace ToolsMandioca.StateMachine
{
    public class Boss_Follow : Range_EnemyStates
    {
        float radiousToAvoidance;
        float avoidWeight;
        //float rotationSpeed;
        Func<Transform> GetMyPos;

        float normalDistance;

        Transform root;

        public Boss_Follow(EState<RangeDummy.RangeDummyInput> myState, EventStateMachine<RangeDummy.RangeDummyInput> _sm,
                                float radAvoid, float voidW, float _rotSpeed, Func<Transform> myPos, float distance, RangeDummy me) : base(myState, _sm, me)
        {
            radiousToAvoidance = radAvoid;
            avoidWeight = voidW;
            GetMyPos += myPos;
            normalDistance = distance;
            //rotationSpeed = _rotSpeed;

            root = me.moveOptions.GetRootTransform();
        }

        protected override void Enter(EState<RangeDummy.RangeDummyInput> input)
        {
            base.Enter(input);
            myDummy.GetAnimator().SetFloat("move", 0.3f);
        }

        protected override void Exit(RangeDummy.RangeDummyInput input)
        {
            base.Exit(input);

            myDummy.GetRigidbody().velocity = Vector3.zero;
            myDummy.GetAnimator().SetFloat("move", 0);
        }

        protected override void Update()
        {
            base.Update();

            if (GetMyPos() == null)
            {
                if (myDummy.CurrentTarget() != null)
                {
                    Vector3 dirForward = (myDummy.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = new Vector3(dirForward.x, 0, dirForward.z);

                    ObstacleAvoidance(fowardRotation);
                    if (Vector3.Distance(myDummy.CurrentTarget().transform.position, root.position) <= normalDistance)
                        sm.SendInput(RangeDummy.RangeDummyInput.IDLE);
                }
            }
            else
            {
                Vector3 dir = GetMyPos().position - root.position;
                dir.Normalize();

                Vector3 dirFix = new Vector3(dir.x, 0, dir.z);

                ObstacleAvoidance(dirFix);

                float distanceX = Mathf.Abs(GetMyPos().transform.position.x - root.position.x);
                float distanceZ = Mathf.Abs(GetMyPos().transform.position.z - root.position.z);

                if (distanceX < 0.7f && distanceZ < 0.7f)
                    sm.SendInput(RangeDummy.RangeDummyInput.IDLE);
            }

        }

        Transform obs;
        protected void ObstacleAvoidance(Vector3 dir)
        {
            obs = null;
            var friends = Physics.OverlapSphere(root.position, radiousToAvoidance);
            if (friends.Length > 0)
            {
                foreach (var item in friends)
                {
                    if (item.GetComponent<EntityBase>())
                    {
                        if (item.GetComponent<EntityBase>() != myDummy)
                        {
                            if (!obs)
                                obs = item.transform;
                            else if (Vector3.Distance(item.transform.position, root.position) < Vector3.Distance(obs.position, root.position))
                                obs = item.transform;
                        }
                    }

                }
            }

            if (obs)
            {
                Vector3 diraux = (root.position - obs.position).normalized;

                diraux = new Vector3(diraux.x,0, diraux.z);

                dir += diraux * avoidWeight;
            }

            var currentspeed = myDummy.moveOptions.GetCurrentSpeed();

            myDummy.GetRigidbody().velocity = new Vector3(dir.x * currentspeed, myDummy.GetRigidbody().velocity.y, dir.z * currentspeed);

            Vector3 forwardRotation = new Vector3(dir.normalized.x, 0, dir.normalized.z);

            root.forward = Vector3.Lerp(root.forward, forwardRotation, myDummy.moveOptions.GetRotationSpeed() * Time.deltaTime);
        }
    }
}
