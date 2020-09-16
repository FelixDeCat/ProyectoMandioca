using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IA2;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOAP
{
    public enum ActionEntity
    {
        UseSkill,
        MeleeAttack,
        Move,
        NextStep,
        Success,
        ThinkPlan,
        FailedStep,
        Idle


    }
    public class Dude : MonoBehaviour
    {
        private EventFSM<ActionEntity> _fsm;
        public Item _target;
        public IEnumerable<Item> auxListTarget;
        public Item auxTarget;

        private Ente _ent;
        private BrainPlanner _planner;
        IEnumerable<Tuple<ActionEntity, Item>> _plan;

        GOAP_Skills_Base _currentSkill;

        public string debugState;

        public void Initialize()
        {
            _ent = GetComponent<Ente>();
            _planner = GetComponent<BrainPlanner>();

            #region Basic States
            var idle = new State<ActionEntity>("idle");
            var any = new State<ActionEntity>("any");
            var planStep = new State<ActionEntity>("planStep");
            var failStep = new State<ActionEntity>("failStep");
            var success = new State<ActionEntity>("success");
            var thinkPlan = new State<ActionEntity>("thinkPlan");
            #endregion

            var meleeAttack = new State<ActionEntity>("meleeAttack");
            var speedBuff = new State<ActionEntity>("speedBuff");
            var move = new State<ActionEntity>("move");
            var useSkill = new State<ActionEntity>("move");

            void NextStep() { _fsm.Feed(ActionEntity.NextStep); };
            void FailedStep() { _fsm.Feed(ActionEntity.FailedStep); };

            useSkill.OnEnter += (a) =>
            {
                _currentSkill = _ent.skillManager.GetSkill(_target.GetComponent<GOAP_Skills_Base>().skillName);
                _currentSkill.Execute();
                if(!_currentSkill.instantSkill)
                {
                    _currentSkill.OnFinishSkill += NextStep;
                    return;
                }

                NextStep();
            };

            useSkill.OnExit += (a) =>
            {
                if (!_currentSkill.instantSkill)
                {
                    _currentSkill.OnFinishSkill -= NextStep;
                }

                _currentSkill = null;
            };

            meleeAttack.OnEnter += (a) =>
            {
                _ent.Stop();
                if (!Check_MeleeRangeHero())
                {
                    FailedStep();
                    return;
                }

                MeleeAttack();

                _ent.OnFinishAttack += NextStep;
                _ent.attackSensor.OnHeroHitted += ResolveAttack;
            };

            meleeAttack.OnExit += (a) =>
            {
                _ent.attackSensor.OnHeroHitted -= ResolveAttack;
                _ent.OnFinishAttack -= NextStep;
            };

            move.OnEnter += (a) =>
            {
                _ent.GoTo(_target.transform.position);
                _ent.OnReachDestinationNoParameters += NextStep;

            };

            move.OnExit += (a) =>
            {
                _ent.OnReachDestinationNoParameters -= NextStep;
            };


            // ////////////
            thinkPlan.OnEnter += a => _planner.StartPlanning();
            //////////////
            failStep.OnEnter += a => { _ent.Stop(); Debug.Log("Plan failed"); };

            //////////////
            planStep.OnEnter += a =>
            {
                var step = _plan.FirstOrDefault();
                if (step != null)
                {
                    _plan = _plan.Skip(1);
                    var oldTarget = _target;
                    _target = step.Item2;
                    if (!_fsm.Feed(step.Item1))
                        _target = oldTarget;
                }
                else
                {
                    _fsm.Feed(ActionEntity.Success);
                }
            };
            //////////////
            success.OnEnter += a =>
            {
                Debug.Log("Success");
                _fsm.Feed(ActionEntity.ThinkPlan);
            };

            StateConfigurer.Create(any)
                .SetTransition(ActionEntity.NextStep, planStep)
                .SetTransition(ActionEntity.ThinkPlan, thinkPlan)
                .SetTransition(ActionEntity.FailedStep, thinkPlan)
                //.SetTransition(ActionEntity.GetDamaged, getDamaged)
                .SetTransition(ActionEntity.Idle, idle)
                .Done();

            StateConfigurer.Create(planStep)
                .SetTransition(ActionEntity.Success, success)
                .SetTransition(ActionEntity.MeleeAttack, meleeAttack)
                .SetTransition(ActionEntity.UseSkill, useSkill)
                .SetTransition(ActionEntity.Move, move)
                .Done();

            _fsm = new EventFSM<ActionEntity>(thinkPlan, any);
        }

        void ResolveAttack(CharacterHead hero)
        {

            var damageData = GetComponent<DamageData>().SetDamage(5).SetDamageType(Damagetype.Normal);

            hero.GetComponent<DamageReceiver>().TakeDamage(damageData);
        }

        bool Check_MeleeRangeHero(){return Vector3.Distance(_target.transform.position, _ent.transform.position) <= 5f ? true:false;}

        void MeleeAttack()
        {
            _ent.Anim().SetTrigger("meleeAttack");
        }


        public void ExecutePlan(List<Tuple<ActionEntity, Item>> plan)
        {
            _plan = plan;
            _fsm.Feed(ActionEntity.NextStep);

        }


        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.P))
                Initialize();

            //Never forget
            if (_fsm != null)
            {
                _fsm.Update();

                debugState = _fsm.Current.Name;
            }


        }
    }
}

