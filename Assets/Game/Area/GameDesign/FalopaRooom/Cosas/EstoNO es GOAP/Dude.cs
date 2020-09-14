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
        Kill,
        PickUp,
        NextStep,
        FailedStep,
        Open,
        Success,
        PressButton,
        PickUpHealth,
        ThinkPlan,
        Idle,
        Collect,
        OpenCoinDoor,
        GetDamaged,
        StandOnPressurePlate,
        AprobarFinal,
        OpenKeyDoor,
        Patrol,
        Chase,
        Hide,


        Speedbuff,
        MeleeAttack,
        Move
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

        public string debugState;



        public void Initialize()
        {
            _ent = GetComponent<Ente>();
            _planner = GetComponent<BrainPlanner>();

            _ent.OnTakeDmg += () => _fsm.Feed(ActionEntity.GetDamaged);

            //estados basicos
            var idle = new State<ActionEntity>("idle");
            var any = new State<ActionEntity>("any");
            var planStep = new State<ActionEntity>("planStep");
            var failStep = new State<ActionEntity>("failStep");
            var success = new State<ActionEntity>("success");
            var thinkPlan = new State<ActionEntity>("thinkPlan");


            var meleeAttack = new State<ActionEntity>("meleeAttack");
            var speedBuff = new State<ActionEntity>("speedBuff");
            var move = new State<ActionEntity>("move");

            void NextStep() { _fsm.Feed(ActionEntity.NextStep); };


            meleeAttack.OnEnter += (a) =>
            {

                _ent.GoTo(_target.transform.position);
                _ent.meleeRange_sensor.On();
                _ent.OnMeleeRangeWithPlayer += MeleeAttack;
                _ent.OnFinishAttack += NextStep;
                _ent.attackSensor.OnHeroHitted += ResolveAttack;
            };

            meleeAttack.OnExit += (a) =>
            {
                _ent.OnMeleeRangeWithPlayer -= MeleeAttack;
                _ent.attackSensor.OnHeroHitted -= ResolveAttack;
                _ent.OnFinishAttack -= NextStep;
                _ent.meleeRange_sensor.Off();
            };

            speedBuff.OnEnter += (a) =>
            {
                SpeedBuff();
                NextStep();
            };

            move.OnEnter += (a) =>
            {
                _ent.GoTo(_target.transform.position);
                //_ent.OnMeleeRangeWithPlayer += NextStep;
                _ent.OnReachDestinationNoParameters += NextStep;

            };

            move.OnUpdate += () =>
            {
                Check_MeleeRangeHero();
            };

            move.OnExit += (a) =>
            {
                _ent.OnMeleeRangeWithPlayer -= NextStep;
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
                .SetTransition(ActionEntity.Speedbuff, speedBuff)
                .SetTransition(ActionEntity.Move, move)
                .Done();

            _fsm = new EventFSM<ActionEntity>(thinkPlan, any);
        }

        void ResolveAttack(CharacterHead hero)
        {

            var damageData = GetComponent<DamageData>().SetDamage(5).SetDamageType(Damagetype.Normal);

            hero.GetComponent<DamageReceiver>().TakeDamage(damageData);
        }

        void Check_MeleeRangeHero()
        {
            if (Vector3.Distance(_target.transform.position, _ent.transform.position) < 2f)
            {
                _fsm.Feed(ActionEntity.NextStep);
            }
        }

        void MeleeAttack()
        {
            _ent.Stop();
            Debug.Log("entro al ataque");
            if (Vector3.Distance(_target.transform.position, _ent.transform.position) > 3)
            {
                _fsm.Feed(ActionEntity.FailedStep);
                return;
            }


            _ent.Anim().SetTrigger("meleeAttack");


        }

        void SpeedBuff()
        {
            _ent.canSpeedBuff = false;
            StartCoroutine(SpeedBuff_Handler());
        }

        IEnumerator SpeedBuff_Handler()
        {
            float auxSpeed = _ent.speed;
            _ent.speed *= 2;

            yield return new WaitForSeconds(5);

            _ent.speed = auxSpeed;

            yield return new WaitForSeconds(10);
            _ent.canSpeedBuff = true;

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

