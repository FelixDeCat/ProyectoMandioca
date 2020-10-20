using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.StateMachine
{
    public class WendigoMelee : WendigoStates
    {
        WendigoView view;
        DamageData dmgData;
        SphereCollider coll;
        public WendigoMelee(EState<WendigoEnemy.WendigoInputs> myState, WendigoView _view, DamageData _dmgData, SphereCollider _coll, EventStateMachine<WendigoEnemy.WendigoInputs> _sm) : base(myState, _sm)
        {
            view = _view;
            dmgData = _dmgData;
            coll = _coll;

        }
        protected override void Enter(EState<WendigoEnemy.WendigoInputs> last)
        {
            view.Kick();
            view.DebugText("MELEE");
        }

        protected override void Update()
        {

            int layerMask = LayerMask.GetMask("Player");
            Collider[] collz = Physics.OverlapSphere(coll.transform.position, coll.radius, layerMask);
            foreach (Collider c in collz)
            {
                Debug.Log(c.gameObject.name);
                if (c.gameObject.layer == 8)
                {
                    dmgData.SetKnockback(1000);
                    Debug.Log(c.gameObject.GetComponent<DamageReceiver>());

                    c.gameObject.GetComponent<DamageReceiver>().TakeDamage(dmgData);
                }
            }
            //Inserte knockback aqui
            //Daño y empujon
            //GEtcomponentdamagereceiver 
            //Take damage -> damage data
        }
    }
}

