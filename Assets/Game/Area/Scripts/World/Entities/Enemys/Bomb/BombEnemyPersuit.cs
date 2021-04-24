using System;
using System.Reflection.Emit;
using UnityEngine;
namespace Tools.StateMachine
{
    public class BombEnemyPersuit : StatesFunctions<BombEnemy.BombInputs>
    {
        GenericEnemyMove move;
        float distanceToExplode;
        Func<EntityBase> Target;
        Animator anim;
        string soundName;
        Transform root;

        public BombEnemyPersuit(EState<BombEnemy.BombInputs> myState, EventStateMachine<BombEnemy.BombInputs> _sm, GenericEnemyMove _move, float _distanceToExplode,
                                Func<EntityBase> _Target, Animator _anim, string _soundName, Transform _root) : base(myState, _sm)
        {
            move = _move;
            distanceToExplode = _distanceToExplode;
            Target = _Target;
            anim = _anim;
            soundName = _soundName;
            root = _root;
        }

        protected override void Enter(EState<BombEnemy.BombInputs> lastState)
        {
            anim.SetFloat("move", 0.3f);

            AudioManager.instance.PlaySound(soundName, anim.transform);
        }

        protected override void Exit(BombEnemy.BombInputs input)
        {
            move.StopMove();
            anim.SetFloat("move", 0);

            AudioManager.instance.StopAllSounds(soundName);
        }

        protected override void FixedUpdate()
        {
        }

        protected override void LateUpdate()
        {
        }

        protected override void Update()
        {
            if (Target() != null)
            {
                Vector3 dirForward = (Target().transform.position - root.position).normalized;
                Vector3 fowardRotation = move.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));

                move.Rotation(fowardRotation.normalized);
                move.MoveWRigidbodyV(fowardRotation);

                if (Vector3.Distance(Target().transform.position, root.position) <= distanceToExplode)
                    sm.SendInput(BombEnemy.BombInputs.PREP_EXPLODE);
            }
            else
                sm.SendInput(BombEnemy.BombInputs.IDLE);
        }
    }
}
