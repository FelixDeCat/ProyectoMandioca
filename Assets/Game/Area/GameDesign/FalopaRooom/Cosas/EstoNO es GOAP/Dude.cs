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
        Idle,
        GetDamaged
    }
    public class Dude : MonoBehaviour, IPauseable
    {
        private EventFSM<ActionEntity> _fsm;
        public Item _target;
        public IEnumerable<Item> auxListTarget;
        public Item auxTarget;

        private Ente _ent;
        private BrainPlanner _planner;
        IEnumerable<Tuple<ActionEntity, Item>> _plan;

        float _auxCount;

        GOAP_Skills_Base _currentSkill;

        bool paused = false;

        public string debugState;

        public void Initialize()
        {
            _ent = GetComponent<Ente>();
            _planner = GetComponent<BrainPlanner>();


            _ent.OnTakeDmg += () => _fsm.Feed(ActionEntity.GetDamaged);


            #region Basic States
            var idle = new State<ActionEntity>("idle");
            var any = new State<ActionEntity>("any");
            var planStep = new State<ActionEntity>("planStep");
            var failStep = new State<ActionEntity>("failStep");
            var success = new State<ActionEntity>("success");
            var thinkPlan = new State<ActionEntity>("thinkPlan");
            var getDamaged = new State<ActionEntity>("getDamaged");
            #endregion

            var meleeAttack = new State<ActionEntity>("meleeAttack");
            var move = new State<ActionEntity>("move");
            var useSkill = new State<ActionEntity>("useSkill");



            /// /// /// ///
            void NextStep() { _fsm.Feed(ActionEntity.NextStep); };
            void FailedStep() { _fsm.Feed(ActionEntity.FailedStep); };

            useSkill.OnEnter += (a) =>
            {
                _currentSkill = _ent.skillManager.GetSkill(_target.GetComponent<GOAP_Skills_Base>().skillName);
                Debug.Log("uso el skill " + _currentSkill.skillName);

                if (!_currentSkill.isAvaliable)
                {
                    FailedStep();
                    return;
                }

                if (!Check_SkillRange(_currentSkill.range))
                {
                    Debug.Log("EL SKILL NO LLEGA");
                    FailedStep();
                    return;
                }

                if (!_currentSkill.stopSkillByCode)
                {
                    _ent.OnFinishSkill += _currentSkill.EndSkill;
                    _ent.OnFinishSkill += NextStep;
                }
                else
                {
                    _currentSkill.OnFinishSkill += NextStep;
                }
                
                _currentSkill.Execute();
            };

            useSkill.OnExit += (a) =>
            {

                if (!_currentSkill.stopSkillByCode)
                {
                    _ent.OnFinishSkill -= _currentSkill.OnFinishSkill;
                    _ent.OnFinishSkill -= NextStep;
                }
                else
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
                //_ent.OnReachDestinationNoParameters += NextStep;
                _auxCount = 0;

            };

            move.OnUpdate += () =>
            {
                _auxCount += Time.deltaTime;

                if (_auxCount >= 3)
                {
                    _ent.Stop();
                    NextStep();
                }
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

            getDamaged.OnEnter += a =>
            {
                 _ent.Anim().Play("GetDamage");
                FailedStep();
            };

            StateConfigurer.Create(any)
                .SetTransition(ActionEntity.NextStep, planStep)
                .SetTransition(ActionEntity.ThinkPlan, thinkPlan)
                .SetTransition(ActionEntity.FailedStep, thinkPlan)
                .SetTransition(ActionEntity.GetDamaged, getDamaged)
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
        bool Check_SkillRange(float range){return Vector3.Distance(Main.instance.GetChar().Root.position, _ent.transform.position) <= range ? true:false; }

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
            if(Input.GetKeyDown(KeyCode.K))
            {
                Initialize();
            }

            if (paused) return;

            //Never forget
            if (_fsm != null)
            {
                _fsm.Update();

                debugState = _fsm.Current.Name;
            }


        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }
    }
}

