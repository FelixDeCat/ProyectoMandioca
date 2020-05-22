using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolsMandioca.StateMachine
{
    public class CharBlock : CharacterStates
    {
        public CharBlock(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm,LockOn myLockOn) : base(myState, _sm)
        {
            _myLockOn = myLockOn;
        }

        protected override void Enter(EState<CharacterHead.PlayerInputs> input)
        {
            charBlock.SetOnBlock(true);

            //if (_myLockOn.isLockOn())
            //{
            //    //nuestro funcioanmiento nuevo
            //    charMove.MovementHorizontal(input / 2);
            //    charMove.MovementVertical(input / 2);
            //}
            //else
            //{
            //    charMove.MovementHorizontal(0);
            //    charMove.MovementVertical(0);
            //}
            charMove.MovementHorizontal(0);
            charMove.MovementVertical(0);
        }

        protected override void Update()
        {
            if (_myLockOn.isLockOn())
            {
                Transform myTransform = charMove.GetTransformRotation();
                EnemyBase myEnemy = _myLockOn.GetCurrentEnemy();
                if (myEnemy)
                {
                    Vector3 enemyPos = new Vector3(myEnemy.transform.position.x, 0f, myEnemy.transform.position.z);
                    myTransform.LookAt(enemyPos);
                }
                    
            }
            else
            {
                charMove.RotateHorizontal(RightHorizontal());
                charMove.RotateVertical(RightVertical());
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
