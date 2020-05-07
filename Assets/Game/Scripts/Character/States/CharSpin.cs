using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProyectTools.StateMachine
{
    public class CharSpin : CharacterStates
    {
        float speedSpin;
        float timerspin = 0;
        Func<float> GetTimeToSpin;
        Func<float> GetSpeedToSpin;
        bool anim;
        GameObject go_feedback;
       Sensor enemy_damage_sensor;
        public CharSpin(EState<CharacterHead.PlayerInputs> myState, EventStateMachine<CharacterHead.PlayerInputs> _sm) : base(myState, _sm)
        {
        }
        public CharSpin Configurate(Func<float> Callbalck_GetTime, Func<float> Callback_GetSpeed, GameObject _go_feedback, Sensor sensor)
        {
            enemy_damage_sensor = sensor;
            enemy_damage_sensor.AddCallback_OnTriggerEnter(DealDamage);
            enemy_damage_sensor.Off();
            go_feedback = _go_feedback;
            go_feedback.SetActive(false);
            GetTimeToSpin = Callbalck_GetTime;
            GetSpeedToSpin = Callback_GetSpeed;
            return this;
        }
        void DealDamage(GameObject go)
        {
            var ent = go.GetComponent<EntityBase>();
            if (ent == null) return;
           
            ent.TakeDamage(5, Main.instance.GetChar().transform.position, Damagetype.normal);
        }


        protected override void Enter(EState<CharacterHead.PlayerInputs> input) { Debug.Log("Begin spin"); charAnim.BeginSpin(ON_END_BeginSpin); }
        void ON_END_BeginSpin() { anim = true; go_feedback.SetActive(true); charMove.CancelRotation(); enemy_damage_sensor.On(); }
        void EndSpin() { charAnim.EndSpin(ON_END_EndSpin); go_feedback.SetActive(false); enemy_damage_sensor.Off(); }
        void ON_END_EndSpin() => sm.SendInput(CharacterHead.PlayerInputs.STUN);
        protected override void Update()
        {
            charMove.MovementHorizontal(LeftHorizontal());
            charMove.MovementVertical(LeftVertical());
            if (anim)
            {
                if (timerspin < GetTimeToSpin())
                {
                    var rot = charMove.GetTransformRotation();
                    rot.Rotate(0, GetSpeedToSpin(), 0);
                    timerspin = timerspin + 1 * Time.deltaTime;
                }
                else
                {
                    timerspin = 0;
                    anim = false;
                    EndSpin();
                }
            }
        }

        protected override void FixedUpdate() { }
        protected override void LateUpdate() { }
        protected override void Exit(CharacterHead.PlayerInputs input)
        {
            charMove.EnableRotation();
        }
    }
}

