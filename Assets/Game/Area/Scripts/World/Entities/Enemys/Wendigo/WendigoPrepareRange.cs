using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{

    public class WendigoPrepareRange : WendigoStates
    {
        GenericEnemyMove moveComp;
        CombatDirectorElement enemy;
        float viewTime;
        float throwTime;
        public WendigoPrepareRange(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, GenericEnemyMove _moveComp, CombatDirectorElement _enem, EventStateMachine<WendigoEnemy.WendigoInputs> _sm, float _throwTime) : base(myState, _sm)
        {
            view = _view;
            moveComp = _moveComp;
            enemy = _enem;
            throwTime = _throwTime;
        }

        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            viewTime = 0f;
            view.DebugText("PrepareRange");
        }
        protected override void Update()
        {

            viewTime += Time.deltaTime;
            if (viewTime > throwTime)
            {
                sm.SendInput(WendigoEnemy.WendigoInputs.RANGEAR);
            }
            if (enemy.CurrentTarget() != null)
            {
                Vector3 dirForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 fowardRotation = moveComp.ObstacleAvoidance(new Vector3(dirForward.x, 0, dirForward.z));
                moveComp.Rotation(fowardRotation.normalized);
            }
        }
    }

}