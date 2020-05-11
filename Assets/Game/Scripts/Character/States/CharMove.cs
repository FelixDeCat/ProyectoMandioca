using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharMove : CharacterStates
    {
        
        public CharMove(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm,LockOn lockon) : base(myState, _sm)
        {
            _myLockOn = lockon;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            base.Enter(input);
        }

        protected override void Update()
        {
            if (_myLockOn.isLockOn())
            {

                Debug.Log("IS LOCKON ON");

                Transform _myTransform = charMove.GetTransformRotation();
                EnemyBase _myEnemy = _myLockOn.GetCurrentEnemy();
                if (_myEnemy)
                {
                    Vector3 enemypos = new Vector3(_myEnemy.transform.position.x, 0, _myEnemy.transform.position.z);
                    _myTransform.LookAt(enemypos);
                }
               
                
            }
            else
            {
                Debug.Log("IS LOCKON OFF");
                charMove.RotateHorizontal(RightHorizontal());
                charMove.RotateVertical(RightVertical());
            }
            
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());

            if (LeftVertical() == 0 && LeftHorizontal() == 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.IDLE);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        protected override void Exit(CharacterHead.PlayerInputs input)
        {

        }
    }
}
