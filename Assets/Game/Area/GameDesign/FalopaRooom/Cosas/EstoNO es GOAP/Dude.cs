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


        GetMeleeRange,
        MeleeAttack
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
        public ParticleSystem pastafrola;
        public ParticleSystem damage;
        public ParticleSystem health;
        public ParticleSystem stealth;
        public string debugState;

        private Item key;

        private void Awake()
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


            var getMeleeRange = new State<ActionEntity>("getMeleeRange");
            var meleeAttack = new State<ActionEntity>("meleeAttack");

            meleeAttack.OnEnter += (a) =>
            {
                _ent.GoTo(_target.transform.position);
                _ent.OnReachDestination += MeleeAttack;
            };

            meleeAttack.OnExit += (a) =>
            {
                _ent.OnReachDestination -= MeleeAttack;
            };



            #region ejemplos
            //estados custom
            //var pickKey = new State<ActionEntity>("pickUpKey");
            //var pressButton = new State<ActionEntity>("pressButton");
            //var pickLife = new State<ActionEntity>("pickUpHealth");

            //var collectCoins = new State<ActionEntity>("collectCoins");
            //var openCoinDoor = new State<ActionEntity>("openCoinDoor");
            //var getDamaged = new State<ActionEntity>("getDamaged");
            //var standOnPressurePlate = new State<ActionEntity>("standOnPressurePlate");
            //var openKeyDoor = new State<ActionEntity>("openKeyDoor");
            //var aprobarFinal = new State<ActionEntity>("aprobarFinal");
            //var hide = new State<ActionEntity>("hide");

            //////////////
            //hide.OnEnter += (a) =>
            //{
            //    _ent.OnReachDestination += WaitInHide;
            //    _ent.GoTo(_target.transform.position);
            //};

            //hide.OnExit += (a) => _ent.OnReachDestination -= WaitInHide;
            ////////////////
            //aprobarFinal.OnEnter += (a) =>
            //{
            //    _ent.OnHitItem += TurnOnPastafrolaCelebration;
            //    _ent.GoTo(_target.transform.position);
            //};

            //aprobarFinal.OnExit += (a) => _ent.OnHitItem -= TurnOnPastafrolaCelebration;
            ////////////////
            //openKeyDoor.OnEnter += (a) =>
            //{
            //    _ent.OnHitItem += OpenKeyDoor;
            //    _ent.GoTo(_target.transform.position);
            //};

            //openKeyDoor.OnExit += (a) =>
            //{
            //    _ent.Stop();
            //    _ent.OnHitItem -= OpenKeyDoor;
            //};
            ////////////////
            //standOnPressurePlate.OnEnter += (a) =>
            //{
            //    _ent.GoTo(_target.transform.position);
            //    _ent.OnStayItem += StandOnPressurePlate;
            //};

            //standOnPressurePlate.OnExit += (a) =>
            //{
            //    _ent.Stop();
            //    _ent.OnStayItem -= StandOnPressurePlate;
            //};
            ////////////////
            //getDamaged.OnEnter += (a) =>
            //{
            //    _ent.Stop();
            //    damage.Play();
            //    _fsm.Feed(ActionEntity.FailedStep);
            //};
            ////////////////
            //openCoinDoor.OnEnter += (a) =>
            //{
            //    _ent.OnHitItem += OpenCoinDoor; _ent.GoTo(_target.transform.position);
            //};

            //openCoinDoor.OnExit += (a) => _ent.OnHitItem -= OpenCoinDoor;
            ////////////////
            //collectCoins.OnEnter += (a) =>
            //{
            //    var aux = WorldState.instance.allItems.Select(x => x.GetComponent<Coin>()).Where(x => x != null);
            //    var g = aux.Skip(Random.Range(0, aux.Count())).FirstOrDefault();

            //    _ent.GoTo(g.transform.position);
            //    _ent.OnHitItem += CollectCoin;
            //};

            //collectCoins.OnExit += a => _ent.OnHitItem -= CollectCoin;
            ////////////////

            ////////////////
            //pressButton.OnEnter += a => { _ent.GoTo(_target.transform.position); _ent.OnHitItem += PressButton; };

            //pressButton.OnExit += a => { _ent.OnHitItem -= PressButton; };
            ////////////////
            //pickKey.OnEnter += a =>
            //{
            //    _ent.GoTo(_target.transform.position);
            //    _ent.OnHitItem += PickKey;
            //};

            //pickKey.OnExit += (a) =>
            //{
            //    _ent.Stop();
            //    _ent.OnHitItem -= PickKey;
            //};
            ////////////////
            //pickLife.OnEnter += a =>
            //{
            //    var closestLife = WorldState.instance.allItems.Where(x => x.type == ItemType.Hp)
            //        .OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();

            //    _ent.GoTo(closestLife.transform.position); _ent.OnHitItem += PickUphealth;
            //};

            //pickLife.OnExit += a => { _ent.OnHitItem -= PickUphealth; };
            #endregion

            // ////////////
            thinkPlan.OnEnter += a => _planner.StartPlanning();
            //////////////
            failStep.OnEnter += a => { _ent.Stop(); Debug.Log("Plan failed"); };

            //////////////
            planStep.OnEnter += a => {
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
            success.OnEnter += a => {
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
                //.SetTransition(ActionEntity.PressButton, pressButton)
                //.SetTransition(ActionEntity.OpenCoinDoor, openCoinDoor)
                //.SetTransition(ActionEntity.PickUp, pickKey)
                //.SetTransition(ActionEntity.PickUpHealth, pickLife)
                //.SetTransition(ActionEntity.Collect, collectCoins)
                //.SetTransition(ActionEntity.StandOnPressurePlate, standOnPressurePlate)
                //.SetTransition(ActionEntity.OpenKeyDoor, openKeyDoor)
                //.SetTransition(ActionEntity.Hide, hide)
                //.SetTransition(ActionEntity.AprobarFinal, aprobarFinal)
                .Done();

            _fsm = new EventFSM<ActionEntity>(thinkPlan, any);
        }

        #region ejemplos acciones

        //private void Start()
        //{
        //    ////Para cuando la gorra lo ve
        //    //WorldState.instance.OnDudeSpotted += () => _fsm.Feed(ActionEntity.ThinkPlan);
        //}

        void MeleeAttack(Ente e, Waypoint wp, bool b)
        {
            _ent.Anim().SetTrigger("meleeAttack");
        }

        //IEnumerator HideTime(float secs)
        //{
        //    WorldState.instance.values["spottedDude"] = false;
        //    _ent.hidden = true;
        //    stealth.Play();
        //    WorldState.instance.OnDudeHidden?.Invoke();

        //    float timeCount = 0;

        //    while (timeCount <= secs)
        //    {
        //        timeCount += Time.deltaTime;
        //        yield return new WaitForEndOfFrame();

        //    }

        //    _fsm.Feed(ActionEntity.ThinkPlan);
        //    _ent.hidden = false;
        //}

        //void TurnOnPastafrolaCelebration(Ente us, Item other)
        //{
        //    if (other.type != ItemType.PastaFrola) return;

        //    pastafrola.Play();
        //    _fsm.Feed(ActionEntity.NextStep);
        //}

        //void OpenKeyDoor(Ente us, Item other)
        //{

        //    if (other.type != ItemType.KeyDoor) return;

        //    var door = other.GetComponent<Door>();

        //    if (door && WorldState.instance.values["hasKey"])
        //    {
        //        Debug.Log("abro puerta de llave");
        //        door.Open();
        //        WorldState.instance.allItems.Remove(other);
        //        WorldState.instance.values["keyDoor"] = false;
        //        WorldState.instance.values["finalReachable"] = true;
        //        _fsm.Feed(ActionEntity.NextStep);
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }
        //}

        //void PickKey(Ente us, Item other)
        //{
        //    if (other.type != ItemType.Key) return;

        //    //other.gameObject.transform.SetParent(us.transform);
        //    key = other;
        //    Destroy(other.GetComponentInChildren<Collider>());
        //    WorldState.instance.values["hasKey"] = true;
        //    _fsm.Feed(ActionEntity.NextStep);

        //}

        //void StandOnPressurePlate(Ente us, Item other)
        //{
        //    if (other.type != ItemType.PressurePlate) return;

        //    var plate = other.GetComponent<PressurePlate>();

        //    if (plate)
        //    {
        //        _ent.transform.position = plate.pressureSpot.position;
        //        WorldState.instance.pressurePlate += Time.deltaTime;
        //        plate.bar.fillAmount = WorldState.instance.pressurePlate / 5f;

        //        if (WorldState.instance.pressurePlate >= 5)
        //        {
        //            plate.RemoveBars();
        //            plate.interactuable = false;
        //            _fsm.Feed(ActionEntity.NextStep);
        //        }
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }
        //}
        //void OpenCoinDoor(Ente us, Item other)
        //{
        //    Debug.Log("abro puerta de monedas");
        //    if (other.type != ItemType.CoinDoor) return;

        //    var door = other.GetComponent<Door>();

        //    if (door && _ent.coins >= 3)
        //    {
        //        door.Open();
        //        WorldState.instance.allItems.Remove(other);
        //        WorldState.instance.values["secondcloseDoor"] = false;
        //        _fsm.Feed(ActionEntity.NextStep);
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }
        //}

        //void CollectCoin(Ente us, Item other)
        //{
        //    Debug.Log("agarro moneda");
        //    if (other.type != ItemType.Coin) return;

        //    var coin = other.GetComponent<Coin>();

        //    if (coin)
        //    {
        //        other.interactuable = false;
        //        WorldState.instance.allItems.Remove(other);
        //        coin.PickCoin();
        //        _ent.coins++;
        //        _fsm.Feed(ActionEntity.NextStep);
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }

        //}

        //void PressButton(Ente us, Item other)
        //{
        //    Debug.Log("apreto boton");
        //    if (other != _target) return;

        //    var button = other.GetComponent<DoorButton>();
        //    if (button)
        //    {
        //        if (button.Press())
        //        {
        //            WorldState.instance.values["firstcloseDoor"] = false;
        //            WorldState.instance.allItems.Remove(button.ReferencedDoor());
        //            _fsm.Feed(ActionEntity.NextStep);
        //        }
        //        else
        //        {
        //            _ent.ModifyHealth(-3);
        //        }
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }
        //}

        //void PickUphealth(Ente us, Item other)
        //{

        //    if (other.type != ItemType.Hp) return;

        //    var life = other.GetComponent<LifePickUp>();
        //    if (life)
        //    {
        //        Debug.Log("agarro vida");
        //        life.PickUpLife();
        //        us.ModifyHealth(5);
        //        health.Play();
        //        WorldState.instance.allItems.Remove(other);
        //        _fsm.Feed(ActionEntity.ThinkPlan);
        //    }
        //    else
        //    {
        //        _fsm.Feed(ActionEntity.FailedStep);
        //    }
        //}

        //public void Spotted()
        //{
        //    _fsm.Feed(ActionEntity.ThinkPlan);
        //}

        #endregion


        public void ExecutePlan(List<Tuple<ActionEntity, Item>> plan)
        {
            _plan = plan;
            _fsm.Feed(ActionEntity.NextStep);

        }    


        private void Update()
        {
            //Never forget
            _fsm.Update();

            debugState = _fsm.Current.Name;

        }
    }
}

