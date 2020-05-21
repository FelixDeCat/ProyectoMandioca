using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : Usable
    {
        [Header("Configuraciones del arma\n asd")]
        [Space(20)]
        public int damage = 50;
        public TrailRenderer mytrail;
       // public WeaponSpecial special;

        public abstract void OnUpdate();

        public override void Equip()
        {
            base.Equip();
            //special = GetComponent<WeaponSpecial>();
            //if (special != null)
            //{
            //    special.OnInitializeSpecial();
            //    special.OnBegin_Passive();
            //}
        }
        public override void UnEquip()
        {
            base.UnEquip();
            //if (special != null)
            //{
            //    special.OnEnd_Passive();
            //    special.OnEnd_Active();
            //}

        }

        //REGULAR ATTACK
        public abstract void OnAttackSleep();

        //POWER UP
        public void BeginSpecial()
        {
            //if (special != null) special.OnBegin_Active();
            OnBeginSpecial();
        }
        public void UpdateSpecial()
        {
            //if (special != null)
            //{
            //    special.OnUpdate_Active();
            //    special.OnUpdate_Passive();
            //}
            OnUpdateSpecial();
        }

        public void Hit(GameObject go)
        {
            //if (special != null)
            //{
            //    Debug.Log("Trengo specialk");
            //    special.OnHit(go);
            //}
            OnHit(go);
        }

        public void EndESpecial()
        {
            //if (special != null)
            //{
            //    special.OnEnd_Active();
            //}
            OnEndSpecial();
        }

        public abstract void OnBeginSpecial();
        public abstract void OnUpdateSpecial();
        public abstract void OnEndSpecial();
        public abstract void OnSensorCollisionDetection(GameObject go);
        public abstract void OnHit(GameObject go);
    }
}
