﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.StateMachine
{
    public class CrowAttack : CrowStates
    {
        CombatDirectorElement enemy;
        Action EnterAction;
        Func<bool> IsCd;
        Action<bool> ActualizeCD;
        float rotationSpeed;
        AudioClip chargeSound;
        Transform myTransform;

        public CrowAttack(EState<CrowEnemy.CrowInputs> myState, EventStateMachine<CrowEnemy.CrowInputs> _sm, Action _EnterAction,
            Func<bool> _IsCd, Action<bool> _ActualizeCD, CombatDirectorElement _enemy, float _rotationSpeed,AudioClip chargeSFX,Transform mytrans) : base(myState, _sm)
        {
            enemy = _enemy;
            EnterAction = _EnterAction;
            IsCd = _IsCd;
            ActualizeCD = _ActualizeCD;
            rotationSpeed = _rotationSpeed;
            chargeSound = chargeSFX;
            myTransform = mytrans;
        }

        protected override void Enter(EState<CrowEnemy.CrowInputs> last)
        {
            base.Enter(last);
            EnterAction();
            AudioManager.instance.PlaySound(chargeSound.name, myTransform);
        }

        protected override void Exit(CrowEnemy.CrowInputs input)
        {
            base.Exit(input);

            enemy.Attacking = false;
            combatDirector.AttackRelease(enemy, enemy.CurrentTarget());
            ActualizeCD(false);
            cdModule.EndCDWithoutExecute("AttackRecall");
        }

        protected override void Update()
        {
            base.Update();

            if (!IsCd() && enemy.CurrentTarget())
            {
                Vector3 myForward = (enemy.CurrentTarget().transform.position - root.position).normalized;
                Vector3 forwardRotation = new Vector3(myForward.x, 0, myForward.z);
                root.forward = Vector3.Lerp(root.forward, forwardRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
