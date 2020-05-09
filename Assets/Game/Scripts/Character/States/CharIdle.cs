using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharIdle : CharacterStates
    {
        public CharIdle(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm,LockOn myLockon) : base(myState, _sm)
        {
            _myLockOn = myLockon;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
        }

        protected override void Update()
        {
            if (_myLockOn.isLockOn())
            {
                Transform myTransfrom = charMove.GetTransformRotation();
                EnemyBase myenemy = _myLockOn.GetCurrentEnemy();
                if (myenemy)
                {
                    Vector3 enemypos = new Vector3(myenemy.transform.position.x, 0, myenemy.transform.position.z);
                    myTransfrom.LookAt(enemypos);
                }
                    

            }
            else
            {
                charMove.RotateHorizontal(RightHorizontal());
                charMove.RotateVertical(RightVertical());
            }
            

            if(LeftHorizontal()!=0 || LeftVertical() != 0)
            {
                sm.SendInput(CharacterHead.PlayerInputs.MOVE);
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
            base.Exit(input);
        }
    }
}
