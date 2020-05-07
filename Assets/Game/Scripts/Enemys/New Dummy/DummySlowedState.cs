using System;
using System.Collections;
using System.Collections.Generic;
using ToolsMandioca.StateMachine;
using UnityEngine;

public class DummySlowedState : DummyFollowState
{
        //Slowed
        float _currentAnimVel;
        float timer;
        float _freezeTime;
        private float _percentSlowed;
        private Material[] originalMats;
        private Material _freezeShader;
             
    
    
        //Follow
        float radiousToAvoidance;
        float avoidWeight;
        float rotationSpeed;
        Func<float> GetSpeed;
        Func<Transform> GetMyPos;

        TrueDummyEnemy noObs;

        float normalDistance;

        public DummySlowedState(float freezeTime, float percentSlowed, Material freezeShader,EState<TrueDummyEnemy.DummyEnemyInputs> myState, EventStateMachine<TrueDummyEnemy.DummyEnemyInputs> _sm,
                                float radAvoid, float voidW, float _rotSpeed, Func<float> speed, Func<Transform> myPos, float distance, TrueDummyEnemy me) 
            : base(myState, _sm, radAvoid, voidW, _rotSpeed, speed, myPos, distance, me)
        {
            radiousToAvoidance = radAvoid;
            avoidWeight = voidW;
            GetSpeed += speed;
            GetMyPos += myPos;
            normalDistance = distance;
            noObs = me;
            rotationSpeed = _rotSpeed;
            
            //freeze settings
            _percentSlowed = percentSlowed;
            _freezeTime = freezeTime;
            _freezeShader = freezeShader;


        }

        protected override void Enter(EState<TrueDummyEnemy.DummyEnemyInputs> input)
        {
            base.Enter(input);
            anim.SetFloat("move", 0.3f);
            
            //freeze Settings
            _currentAnimVel = anim.speed;
            anim.speed = _currentAnimVel * _percentSlowed;
            
            var smr = noObs.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null)
            {
                originalMats = smr.materials;
            
                Material[] mats = smr.materials;
                mats[0] = _freezeShader;
                smr.materials = mats;
            }
            
        }

        protected override void Exit(TrueDummyEnemy.DummyEnemyInputs input)
        {
            base.Exit(input);

            rb.velocity = Vector3.zero;
            anim.SetFloat("move", 0);
            //setear animación
            
            
            //freeze settings
            anim.speed = _currentAnimVel;
            timer = 0;
            ReturnToOriginalMats();
        }
        
        //Cambio el shader que habia por el original
        public void ReturnToOriginalMats()
        {
            var smr = noObs.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null)
            {
                Material[] mats = smr.materials;
                smr.materials = originalMats;
            }

        }

        protected override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= _freezeTime)
            {
                sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                timer = 0;
            }

            
            //de aca para abajo es el follow normal menos un calculo al final
            if (GetMyPos() == null)
            {
                if (noObs.CurrentTarget() != null)
                {
                    Vector3 dirForward = (noObs.CurrentTarget().transform.position - root.position).normalized;
                    Vector3 fowardRotation = new Vector3(dirForward.x, 0, dirForward.z);

                    ObstacleAvoidance(fowardRotation);
//                    if (Vector3.Distance(noObs.CurrentTarget().transform.position, root.position) <= normalDistance)
//                        sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
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
                {
                    //sm.SendInput(TrueDummyEnemy.DummyEnemyInputs.IDLE);
                }
            }

        }

        Transform obs;
        //protected void ObstacleAvoidance(Vector3 dir)
        //{
        //    obs = null;
        //    var friends = Physics.OverlapSphere(root.position, radiousToAvoidance);
        //    if (friends.Length > 0)
        //    {
        //        foreach (var item in friends)
        //        {
        //            if (item.GetComponent<EntityBase>())
        //            {
        //                if (item.GetComponent<EntityBase>() != noObs)
        //                {
        //                    if (!obs)
        //                        obs = item.transform;
        //                    else if (Vector3.Distance(item.transform.position, root.position) < Vector3.Distance(obs.position, root.position))
        //                        obs = item.transform;
        //                }
        //            }

        //        }
        //    }

        //    if (obs)
        //    {
        //        Vector3 diraux = (root.position - obs.position).normalized;
        //        //diraux

        //        diraux = new Vector3(diraux.x,0, diraux.z);

        //        dir += diraux * avoidWeight;
        //    }
        //    //aca se le multiplica la GetSpeed con el percent de slowed
        //    rb.velocity = new Vector3(dir.x * GetSpeed() * _percentSlowed, rb.velocity.y, dir.z * GetSpeed());

        //    Vector3 forwardRotation = new Vector3(dir.normalized.x, 0, dir.normalized.z);

        //    root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);
        //}
}
