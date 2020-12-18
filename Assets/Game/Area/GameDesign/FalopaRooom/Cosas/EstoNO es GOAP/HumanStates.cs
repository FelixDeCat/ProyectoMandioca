using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IA2;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOAP
{
    public enum HumanActions
    {
        UseSkill,
        MeleeAttack,
        Move,
        NextStep,
        Success,
        ThinkPlan,
        FailedStep,
        Idle,
        GetDamaged,
        Avoid
    }

    [System.Serializable]
    public class HumanStates : EntityData
    {
        private EventFSM<HumanActions> _fsm;
        public Item _target;
        public IEnumerable<Item> auxListTarget;
        public Item auxTarget;

        IEnumerable<Tuple<HumanActions, Item>> _plan;

        public float _auxCount;

        GOAP_Skills_Base _currentSkill;

        bool initialized = false;

        public string debugState;

        public void Initialize()
        {
            if (initialized)
            {
                _fsm.Feed(HumanActions.ThinkPlan);
                return;
            }

            initialized = true;

            Human.OnTakeDmg += () => _fsm.Feed(HumanActions.GetDamaged);
            //_planner.OnCantPlan += () => _fsm.Feed(ActionEntity.ThinkPlan);

            #region Basic States
            var idle = new State<HumanActions>("idle");
            var any = new State<HumanActions>("any");
            var planStep = new State<HumanActions>("planStep");
            var failStep = new State<HumanActions>("failStep");
            var success = new State<HumanActions>("success");
            var thinkPlan = new State<HumanActions>("thinkPlan");
            var getDamaged = new State<HumanActions>("getDamaged");
            #endregion

            var meleeAttack = new State<HumanActions>("meleeAttack");
            var move = new State<HumanActions>("move");
            var useSkill = new State<HumanActions>("useSkill");
            var avoid = new State<HumanActions>("avoid");


            /// /// /// ///
            void NextStep() {  _fsm.Feed(HumanActions.NextStep); };
            void FailedStep() { _fsm.Feed(HumanActions.FailedStep); };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ USE SKILL ]
            useSkill.OnEnter += (a) =>
            {
                _currentSkill = Skills.GetSkill(_target.GetComponent<GOAP_Skills_Base>().skillName);
                Debug.Log("uso el skill " + _currentSkill.skillName);

                if (!_currentSkill.isAvaliable)
                {
                    Debug.Log(_currentSkill.skillName + " no esta disponible");
                    FailedStep();
                    return;
                }

                if (!Check_SkillRange(_currentSkill.range))
                {
                    Debug.Log("EL SKILL NO LLEGA");
                    FailedStep();
                    return;
                }

                if(!_currentSkill.ExternalCondition())
                {
                    Debug.Log("La condicion fallo");
                    FailedStep();
                    return;
                }

                if (!_currentSkill.stopSkillByCode)
                {
                    Human.OnFinishSkill += _currentSkill.EndSkill;
                    Human.OnFinishSkill += NextStep;
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
                    Human.OnFinishSkill -= _currentSkill.OnFinishSkill;
                    Human.OnFinishSkill -= NextStep;
                }
                else
                {
                    _currentSkill.OnFinishSkill -= NextStep;
                }

                
                _currentSkill = null;
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ MELEE ATTACK ]
            
            meleeAttack.OnEnter += (a) =>
            {
                Human.Stop();
                if (!Check_MeleeRangeHero())
                {
                    FailedStep();
                    return;
                }

                MeleeAttack();

                Human.OnFinishAttack += NextStep;
                Human.attackSensor.OnHeroHitted += ResolveAttack;
            };

            meleeAttack.OnExit += (a) =>
            {
                Human.attackSensor.OnHeroHitted -= ResolveAttack;
                Human.OnFinishAttack -= NextStep;
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ MOVE ]

            move.OnEnter += (a) =>
            {
                Human.GoTo(Navigation.instance.NearestTo(_target.transform.position).transform.position);
                _auxCount = 0;

            };

            move.OnUpdate += () =>
            {
                _auxCount += Time.deltaTime;

                if (_auxCount >= 4)
                {
                    Human.Stop();
                    NextStep();
                }
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ AVOID ]
            
            avoid.OnEnter += (a) =>
            {
                _auxCount = 0;

                Human.GoTo(Navigation.instance.GetFarAwayWp(Navigation.instance.NearestTo(_target.transform.position)).transform.position);
            };

            avoid.OnUpdate += () =>
            {
                _auxCount += Time.deltaTime;

                if (_auxCount >= 3)
                {
                    Human.Stop();
                    NextStep();
                }
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ THINK PLAN ]

            thinkPlan.OnEnter += a => { _auxCount = 0; Brain.StartPlanning(); Debug.Log("A planear"); };

            thinkPlan.OnUpdate += () =>
            {
                _auxCount += Time.deltaTime;
                
                if (_auxCount >= 4)
                {
                    Debug.Log("llego a entrar aca?");
                    _target = Main.instance.GetChar().GetComponent<Item>();
                    _fsm.Feed(HumanActions.Move);
                }
            };

            thinkPlan.OnExit += a => { Brain.StopPlanning(); Debug.Log("Dejo de planear"); };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ FAIL STEP ]

            failStep.OnEnter += a => { Human.Stop(); Debug.Log("Plan failed"); };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ PLAN STEP ]

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
                    _fsm.Feed(HumanActions.Success);
                }
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ SUCESS ]

            success.OnEnter += a =>
            {
                Debug.Log("Success");
                _fsm.Feed(HumanActions.ThinkPlan);
            };

            ///////////////////////////////////////////////////////////////////////////////////////////////// [ GET DAMAGED ]

            getDamaged.OnEnter += a =>
            {
                _plan = null;
                _auxCount = 0;
                Human.Stop();
                Brain.StopPlanning();
                //_ent.Anim().Play("GetDamage");
            };

            getDamaged.OnUpdate += () =>
            {
                _auxCount += Time.deltaTime;

                if (_auxCount >= 4)
                {    
                    FailedStep();
                }
            };

            /////////////////////////////////////////////////////////////////////////////////////////////////

            StateConfigurer.Create(any)
                .SetTransition(HumanActions.NextStep, planStep)
                .SetTransition(HumanActions.ThinkPlan, thinkPlan)
                .SetTransition(HumanActions.FailedStep, thinkPlan)
                .SetTransition(HumanActions.GetDamaged, getDamaged)
                .SetTransition(HumanActions.Idle, idle)
                .SetTransition(HumanActions.Move, move)
                .Done();

            StateConfigurer.Create(planStep)
                .SetTransition(HumanActions.Success, success)
                .SetTransition(HumanActions.MeleeAttack, meleeAttack)
                .SetTransition(HumanActions.UseSkill, useSkill)
                .SetTransition(HumanActions.Move, move)
                .SetTransition(HumanActions.Avoid, avoid)
                //.SetTransition(ActionEntity.GetDamaged, getDamaged)
                .Done();

            _fsm = new EventFSM<HumanActions>(thinkPlan, any);
        }

        //hago esto para dar un toque de tiempo entre next step y next step para poder setear cosas de animator
        IEnumerator SeparationTime()
        {
            yield return new WaitForSeconds(0.1f);
        }

        void ResolveAttack(CharacterHead hero)
        {
            var damageData = Human.GetComponent<DamageData>().SetDamage(5).SetDamageType(Damagetype.Normal);
            hero.GetComponent<DamageReceiver>().TakeDamage(damageData);
        }

        bool Check_MeleeRangeHero(){return Vector3.Distance(_target.transform.position, Human.transform.position) <= 5f ? true:false;}
        bool Check_SkillRange(float range){return Vector3.Distance(Main.instance.GetChar().Root.position, Human.transform.position) <= range ? true:false; }

        void MeleeAttack()
        {
            Human.Anim().SetTrigger("meleeAttack");
        }


        public void ExecutePlan(List<Tuple<HumanActions, Item>> plan)
        {
            foreach (var item in plan)
            {
                Debug.Log("La accion es " + item.Item1 + " y el item es " + item.Item2.name);
            }

            _plan = plan;
            _fsm.Feed(HumanActions.NextStep);

        }

        public override void ManualUpdate()
        {
            if (_fsm != null)
            {
                _fsm.Update();
                debugState = _fsm.Current.Name;
            }
        }

        public void ResetDude()
        {
            _fsm.Feed(HumanActions.Idle);
        }
    }
}

