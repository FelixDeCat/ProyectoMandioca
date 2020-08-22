#region Instrucciones Maquina de Estados
/*
 * ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄
 * ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀

    MAQUINA DE ESTADOS:

        esta maquina de estados, es la version de los profes de Da Vinci, la que usa eventos y lamdas...
        usa un diccionario en el que se guardan las transiciones, estos se componen de 
        <KEY> El tipo de dato que usemos como input, ejemplo un enum, solo para identificar el HASH
        <VALUE> La transicion que adentro guarda el input <T> y el estado al que apunta State<T>

        la cuestion es configurar todas las transiciones con el StateConfigurer que ya viene programado,
        poniendole el input como un estímulo a cambiar de estado y seguido del estado a cambiar.

        una vez configuradas las transiciones, hay que por cada estado decirle que va a hacer cada funcion
        ej OnEnter, OnExit, OnUpdate... Tanto el OnEnter recibe un parametro del estado del que viene como
        el OnExit recibe un parametro al cual va...

        Actualmente el sistema de StateMachine lo complemento con un agregado propio, en el que creo clases
        Monovehaviours correspondientes a su estado... con sus funciones propias de un estado.
        Esto lo hago porque la state machine como está, es útil, pero es muy desprolija. Para esto creo un
        diccionario < State<T> , StateBase<T> >  siendo StateBase<T> un monovehaviour y poder asi aprobecharme
        de la edicion por Editor... Luego en vez de asignale a todos los estados uno por uno lo que va a hacer
        directamente recorro el diccionario y por cada lamda le asigno la funcion correspondiente de su
        contraparte MonoBehaviour... Luego por medio de callbacks voy llamando a funciones que disparan
        los estimulos.

 * ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄
 * ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
 */
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
using IA2;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Enem //: Enemy
{
    //#region variables

    //public UnityEvent EV_Death;

    //[Header("New Enemy")]
    //public float hitforce;
    //[SerializeField] public DebugText debug_estado;

    //Vector3 postoinvestigate;

    //public Light spot_light;

    //public Vector3 firstPosition;
    //public Renderer myrend;

    //public FeedbackEnemy feedbackEnemy;

    //public StateBase<PlayerInput> currentState;

    //public bool ISeeAEnemy;

    //public Enem_AnimController anims;

    //public CircularLife circular_life;

    //public enum PlayerInput
    //{
    //    ENM_ENTER_ROOM,
    //    ENM_EXIT_ROOM,
    //    ENM_ATTACKING_ME,
    //    ENM_ON_LN_SIGHT,
    //    ENM_OUT_LN_SIGHT,
    //    ENM_CONFIRMED,
    //    ENM_RANGE_ATTACK,
    //    ENM_OUT_OF_RANGE,
    //    CAN_ATTACK,
    //    PATROL_ORDER,
    //    ENM_ACTIVE_ME,
    //    ENM_DEACTIVE_ME,
    //    ALARM,
    //    SOMETHING_WRONG,
    //    END_INSPECTION,
    //    TIME_SEARCH_OVER,
    //    ENM_DEATH,
    //    ANTICIPATION_SUCCESSFUL,
    //    ATTACK_SUCCESSFUL,
    //    ENM_BLOCKING,
    //    ENM_SURPRISE_ME,
    //    BACK_MY_POS_SUCCESSFUL,
    //    DEATH,
    //    ON_FIRE_BEGIN,
    //    ON_CONFUSED_BEGIN,
    //    ON_STUNED_BEGIN,
    //    ON_FREEZE_BEGIN,
    //    ON_FIRE_END,
    //    ON_CONFUSED_END,
    //    ON_STUNED_END,
    //    ON_FREEZE_END,
    //    ON_CRITIC,
    //    ON_CRITIC_TIME_END,
    //    ON_RESURRECT,
    //    ON_ANIMATION_RESURRECT_SUCCESSFUL
    //}

    //[Header("States")]
    //public State_Off state_off;
    //public State_Idle state_idle;
    //public State_OnSight state_onsight;
    //public State_Patrol state_patrol;
    //public State_Inspect state_inspect;
    //public State_Alert state_alert;
    //public State_Pursuit state_pursuit;
    //public State_InPosToAttack state_InPosToAttack;
    //public State_Anticipation state_anticipation;
    //public State_Attack state_attack;
    //public State_Search state_search;
    //public State_Return state_return;
    //public State_Stuned state_stuned;
    //public State_OnFire state_onfire;
    //public State_Confused state_confused;
    //public State_Freeze state_freeze;
    //public State_OnCritic state_oncritic;
    //public State_OnDeath state_ondeath;
    //public State_OnResurrect state_onresurrect;

    //public bool noDungeon;

    //public bool ispatroller;

    //Dictionary<IA2.State<PlayerInput>, StateBase<PlayerInput>> dic = new Dictionary<IA2.State<PlayerInput>, StateBase<PlayerInput>>();
    //private IA2.EventFSM<PlayerInput> _myFsm;

    //#endregion

    //#region MonoBehaviour
    //private void FixedUpdate() { if (active) _myFsm.FixedUpdate(); }
    //private void Update()
    //{
    //    if (active) _myFsm.Update();

    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        OnPatrol();
    //    }

    //}
    //#endregion

    //#region Initialization
    ///// <summary>
    ///// Esta es la inicializacion solo del enem... si quisiera inicializarlo bien... hacerlo desde Entity
    ///// </summary>
    //public override void Init()
    //{
    //    circular_life = GetComponentInChildren<CircularLife>();

    //    icon = WorldToUI.instancia.CreateIcon(rb.transform);
    //    icon.Deactivate();

    //    GetStates();
    //    ConfigureStates();
    //    StateMachine();
    //    active = true;
    //}
    //void ConfigureStates()
    //{
    //    state_off.Configurate(Enemy_Enter_In_The_Room, Player_Active_Me, circular_life, spot_light);
    //    state_idle.Configurate(Player_Deactive_Me, LineOfSight, Patrol_Order, SomethingWrong, Alarm, AttackMe, SurpriseMe, rb, this);
    //    state_onsight.Configurate(Player_Deactive_Me, EnemyConfirmed, AttackMe, LoseLineOfSight, EnemyDeath, this, rb);
    //    state_patrol.Configurate(Player_Deactive_Me, LineOfSight, SomethingWrong, Alarm, AttackMe, Ispatroller, rb, this);
    //    state_inspect.Configurate(Player_Deactive_Me, LineOfSight, Alarm, AttackMe, SurpriseMe, EndInspection, rb, this);
    //    state_search.Configurate(Player_Deactive_Me, SomethingWrong, AttackMe, LineOfSight, TimeSearchOver, this);
    //    state_InPosToAttack.Configurate(Player_Deactive_Me, Out_Of_Range_attack, CanAttack, LoseLineOfSight, this, rb);
    //    state_return.Configurate(Player_Deactive_Me, BackToMyPosSuccessful, LineOfSight, SomethingWrong, Alarm, AttackMe, rb, this);
    //    state_pursuit.Configurate(Player_Deactive_Me, In_Range_attack, LoseLineOfSight, this);
    //    state_alert.Configurate(Player_Deactive_Me, Out_Of_Range_attack, In_Range_attack, LoseLineOfSight, this, rb);
    //    state_anticipation.Configurate(Player_Deactive_Me, Out_Of_Range_attack, AnticipationSuccesful, LoseLineOfSight, this);
    //    state_attack.Configurate(Player_Deactive_Me, Out_Of_Range_attack, AttackSussecful, LoseLineOfSight, playerdamage, this);
    //    state_stuned.Configurate(Player_Deactive_Me, OnStunedEnd, this);
    //    state_ondeath.Configurate(this);
    //}
    //void Ispatroller() { ispatroller = true; }
    //void GetStates()
    //{
    //    state_off = this.GetComponentInChildren<State_Off>();
    //    state_idle = this.GetComponentInChildren<State_Idle>();
    //    state_onsight = this.GetComponentInChildren<State_OnSight>();
    //    state_patrol = this.GetComponentInChildren<State_Patrol>();
    //    state_inspect = this.GetComponentInChildren<State_Inspect>();
    //    state_alert = this.GetComponentInChildren<State_Alert>();
    //    state_pursuit = this.GetComponentInChildren<State_Pursuit>();
    //    state_InPosToAttack = this.GetComponentInChildren<State_InPosToAttack>();
    //    state_anticipation = this.GetComponentInChildren<State_Anticipation>();
    //    state_attack = this.GetComponentInChildren<State_Attack>();
    //    state_search = this.GetComponentInChildren<State_Search>();
    //    state_return = this.GetComponentInChildren<State_Return>();
    //    state_stuned = this.GetComponentInChildren<State_Stuned>();
    //    state_onfire = this.GetComponentInChildren<State_OnFire>();
    //    state_confused = this.GetComponentInChildren<State_Confused>();
    //    state_freeze = this.GetComponentInChildren<State_Freeze>();
    //    state_oncritic = this.GetComponentInChildren<State_OnCritic>();
    //    state_ondeath = this.GetComponentInChildren<State_OnDeath>();
    //    state_onresurrect = this.GetComponentInChildren<State_OnResurrect>();
    //}
    //void StateMachine()
    //{
    //    #region STATE MACHINE CONFIGS
    //    ///////////////////////////////////////////////////////////////
    //    ///////////////////////////////////////////////////////////////

    //    #region State creation

    //    var off = new IA2.State<PlayerInput>(CommonState.OFF);
    //    var idle = new IA2.State<PlayerInput>(CommonState.IDLE);
    //    var onSigth = new IA2.State<PlayerInput>(CommonState.ONSIGHT);
    //    var patrol = new IA2.State<PlayerInput>(CommonState.PATROL);
    //    var inspect = new IA2.State<PlayerInput>(CommonState.INSPECT);
    //    var alert = new IA2.State<PlayerInput>(CommonState.ALERT);
    //    var pursuit = new IA2.State<PlayerInput>(CommonState.PURSUIT);
    //    var inposattack = new IA2.State<PlayerInput>(CommonState.INPOSATTACK);
    //    var anticipation = new IA2.State<PlayerInput>(CommonState.ANTICIPATION);
    //    var attacking = new IA2.State<PlayerInput>(CommonState.ATTACKING);
    //    var search = new IA2.State<PlayerInput>(CommonState.SEARCH);
    //    var returntopos = new IA2.State<PlayerInput>(CommonState.RETURN);
    //    var onfire = new IA2.State<PlayerInput>(CommonState.ONFIRE);
    //    var confused = new IA2.State<PlayerInput>(CommonState.ONCONFUSE);
    //    var stuned = new IA2.State<PlayerInput>(CommonState.ONSTUNED);
    //    var freeze = new IA2.State<PlayerInput>(CommonState.ONFREEZE);
    //    var oncritic = new IA2.State<PlayerInput>(CommonState.ONCRITIC);
    //    var death = new IA2.State<PlayerInput>(CommonState.DEATH);
    //    var resurrect = new IA2.State<PlayerInput>(CommonState.RESURRECT);

    //    #endregion

    //    #region State Configurer

    //    StateConfigurer.Create<PlayerInput>(off)
    //        .SetTransition(PlayerInput.ENM_ENTER_ROOM, idle)
    //        .SetTransition(PlayerInput.ENM_ACTIVE_ME, idle)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(idle)
    //        .SetTransition(PlayerInput.ENM_DEACTIVE_ME, off)
    //        .SetTransition(PlayerInput.ENM_ON_LN_SIGHT, onSigth)
    //        .SetTransition(PlayerInput.PATROL_ORDER, patrol)
    //        .SetTransition(PlayerInput.SOMETHING_WRONG, inspect)
    //        .SetTransition(PlayerInput.ALARM, alert)
    //        .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //        .SetTransition(PlayerInput.ENM_SURPRISE_ME, stuned)
    //        .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //        .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //        .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //        .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //        .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //        .SetTransition(PlayerInput.DEATH, death)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(onSigth)
    //        .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //        .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //        .SetTransition(PlayerInput.ALARM, alert)
    //        .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //        .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, search)
    //        .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //        .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //        .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //        .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //        .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //        .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //        .SetTransition(PlayerInput.DEATH, death)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(patrol)
    //        .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //        .SetTransition(PlayerInput.ENM_ON_LN_SIGHT, onSigth)
    //        .SetTransition(PlayerInput.SOMETHING_WRONG, inspect)
    //        .SetTransition(PlayerInput.ALARM, alert)
    //        .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //        .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //        .SetTransition(PlayerInput.ENM_SURPRISE_ME, stuned)
    //        .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //        .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //        .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //        .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //        .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //        .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //        .SetTransition(PlayerInput.DEATH, death)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(inspect)
    //        .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //        .SetTransition(PlayerInput.ENM_ON_LN_SIGHT, onSigth)
    //        .SetTransition(PlayerInput.ALARM, alert)
    //        .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //        .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //        .SetTransition(PlayerInput.END_INSPECTION, returntopos)
    //        .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //        .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //        .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //        .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //        .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //        .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //        .SetTransition(PlayerInput.DEATH, death)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(alert)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ENM_OUT_OF_RANGE, search)
    //         .SetTransition(PlayerInput.ENM_RANGE_ATTACK, inposattack)
    //         .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, search)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(pursuit)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ENM_RANGE_ATTACK, inposattack)
    //         .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, returntopos)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(inposattack)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ENM_OUT_OF_RANGE, pursuit)
    //         .SetTransition(PlayerInput.CAN_ATTACK, anticipation)
    //         .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, pursuit)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(anticipation)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ENM_OUT_OF_RANGE, pursuit)
    //         .SetTransition(PlayerInput.ANTICIPATION_SUCCESSFUL, attacking)
    //         .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, pursuit)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(attacking)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ENM_OUT_OF_RANGE, pursuit)
    //         .SetTransition(PlayerInput.ATTACK_SUCCESSFUL, inposattack)
    //         .SetTransition(PlayerInput.ENM_OUT_LN_SIGHT, pursuit)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ENM_BLOCKING, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(search)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.SOMETHING_WRONG, inspect)
    //         .SetTransition(PlayerInput.ENM_ON_LN_SIGHT, onSigth)
    //         .SetTransition(PlayerInput.TIME_SEARCH_OVER, returntopos)
    //         .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //         .SetTransition(PlayerInput.ALARM, alert)
    //         .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //         .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(returntopos)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.BACK_MY_POS_SUCCESSFUL, idle)
    //         .SetTransition(PlayerInput.PATROL_ORDER, patrol)
    //         .SetTransition(PlayerInput.ENM_ON_LN_SIGHT, onSigth)
    //         .SetTransition(PlayerInput.ENM_CONFIRMED, alert)
    //         .SetTransition(PlayerInput.SOMETHING_WRONG, inspect)
    //         .SetTransition(PlayerInput.ALARM, alert)
    //         .SetTransition(PlayerInput.ENM_ATTACKING_ME, alert)
    //         .SetTransition(PlayerInput.ON_STUNED_BEGIN, stuned)
    //         .SetTransition(PlayerInput.ON_CONFUSED_BEGIN, confused)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(stuned)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ON_STUNED_END, alert)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(onfire)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ON_FIRE_END, alert)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, alert)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(confused)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ON_CONFUSED_END, alert)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, onfire)
    //         .SetTransition(PlayerInput.ON_FREEZE_BEGIN, freeze)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(freeze)
    //         .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //         .SetTransition(PlayerInput.ON_FREEZE_END, alert)
    //         .SetTransition(PlayerInput.ON_FIRE_BEGIN, alert)
    //         .SetTransition(PlayerInput.ON_CRITIC, oncritic)
    //         .SetTransition(PlayerInput.DEATH, death)
    //         .Done();

    //    StateConfigurer.Create<PlayerInput>(oncritic)
    //        .SetTransition(PlayerInput.ENM_DEACTIVE_ME, idle)
    //        .SetTransition(PlayerInput.ON_CRITIC_TIME_END, returntopos)
    //        .SetTransition(PlayerInput.ENM_DEATH, returntopos)
    //        .SetTransition(PlayerInput.DEATH, death)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(death)
    //        .SetTransition(PlayerInput.ON_RESURRECT, resurrect)
    //        .Done();

    //    StateConfigurer.Create<PlayerInput>(resurrect)
    //        .SetTransition(PlayerInput.ON_ANIMATION_RESURRECT_SUCCESSFUL, idle)
    //        .Done();
    //    #endregion

    //    #region Dictionary Configurer < STATE , STATE_BASE<T> >

    //    dic.Add(off, state_off);
    //    dic.Add(idle, state_idle);
    //    dic.Add(onSigth, state_onsight);
    //    dic.Add(patrol, state_patrol);
    //    dic.Add(inspect, state_inspect);
    //    dic.Add(alert, state_alert);
    //    dic.Add(pursuit, state_pursuit);
    //    dic.Add(inposattack, state_InPosToAttack);
    //    dic.Add(anticipation, state_anticipation);
    //    dic.Add(attacking, state_attack);
    //    dic.Add(search, state_search);
    //    dic.Add(returntopos, state_return);
    //    dic.Add(stuned, state_stuned);
    //    dic.Add(onfire, state_onfire);
    //    dic.Add(confused, state_confused);
    //    dic.Add(freeze, state_freeze);
    //    dic.Add(oncritic, state_oncritic);
    //    dic.Add(death, state_ondeath);
    //    dic.Add(resurrect, state_onresurrect);

    //    foreach (var v in dic)
    //    {
    //        v.Key.OnEnter += x => v.Value.OnEnter(x, Send, SetCurrentState);
    //        v.Key.OnFixedUpdate += () => v.Value.OnFixedUpdate();
    //        v.Key.OnUpdate += () => v.Value.OnUpdate();
    //        v.Key.OnExit += x => v.Value.OnExit(x);
    //    }

    //    #endregion

    //    ///////////////////////////////////////////////////////////////
    //    ///////////////////////////////////////////////////////////////
    //    #endregion

    //    _myFsm = new IA2.EventFSM<PlayerInput>(off, debug_estado.Print);
    //}

    //#endregion

    //#region State Machine Functions

    ////////////////////////////////////////////////////////////////////////////////////
    ////// CONFIGURATIONS
    ////////////////////////////////////////////////////////////////////////////////////

    //public void Send(PlayerInput inp) => _myFsm.SendInput(inp);
    //public void SetCurrentState(StateBase<PlayerInput> current) => currentState = current;
    //public void SetPositionToInvestigate(Vector3 pos) { postoinvestigate = pos; }

    ////////////////////////////////////////////////////////////////////////////////////
    ////// INPUT SENDERS / FROM CALLBACKS
    ////////////////////////////////////////////////////////////////////////////////////

    //public void LineOfSight() { Send(PlayerInput.ENM_ON_LN_SIGHT); }
    //public void OnStunedEnd() { Send(PlayerInput.ON_STUNED_END); }
    //public void LoseLineOfSight() { Send(PlayerInput.ENM_OUT_LN_SIGHT); }
    //public void Out_Of_Range_attack() { Send(PlayerInput.ENM_OUT_OF_RANGE); }
    //public void In_Range_attack() { Send(PlayerInput.ENM_RANGE_ATTACK); }
    //public void EndInspection() { Send(PlayerInput.END_INSPECTION); }
    //public void BackToMyPosSuccessful() { SendInputReturn(); }
    //public void CanAttack() { Send(PlayerInput.CAN_ATTACK); }
    //public void AnticipationSuccesful() { Send(PlayerInput.ANTICIPATION_SUCCESSFUL); }
    //public void SomethingWrong() { Send(PlayerInput.SOMETHING_WRONG); }
    //public void TimeSearchOver() { Send(PlayerInput.TIME_SEARCH_OVER); }
    //public void Enemy_Enter_In_The_Room(Transform who) { Send(PlayerInput.ENM_ENTER_ROOM); }
    //public void Player_Active_Me(Transform who) { Send(PlayerInput.ENM_ACTIVE_ME); }
    //public void Player_Deactive_Me() { Send(PlayerInput.ENM_DEACTIVE_ME); }
    //public void AttackSussecful() { Send(PlayerInput.ATTACK_SUCCESSFUL); }
    //public void Patrol_Order() { Send(PlayerInput.PATROL_ORDER); }
    //public void EnemyDeath() { Send(PlayerInput.ENM_DEATH); }
    //public void Alarm() { Send(PlayerInput.ALARM); }
    //public void SurpriseMe() { Send(PlayerInput.ENM_SURPRISE_ME); }
    //public void AttackMe(Transform who) { Debug.LogError("MEATACAN"); Send(PlayerInput.ENM_ATTACKING_ME); }
    //public void EnemyConfirmed() { Send(PlayerInput.ENM_CONFIRMED); }

    ////////////////////////////////////////////////////////////////////////////////////
    ////// INPUT SENDERS / EXTERNAL
    ////////////////////////////////////////////////////////////////////////////////////
    //public override void OnStuned() { try { if ((IStuneable)currentState != null) Send(PlayerInput.ON_STUNED_BEGIN); } catch (InvalidCastException) { } }
    //public void OnAttack() { try { if ((IAtackable)currentState != null) Send(PlayerInput.ENM_ATTACKING_ME); } catch (InvalidCastException) { } }
    //public void OnFire() { try { if ((IFlamable)currentState != null) Send(PlayerInput.ON_FIRE_BEGIN); } catch (InvalidCastException) { } }
    //public void OnConfused() { try { if ((IConfusable)currentState != null) Send(PlayerInput.ON_CONFUSED_BEGIN); } catch (InvalidCastException) { } }
    //public void OnFreeze() { try { if ((IFreezeable)currentState != null) Send(PlayerInput.ON_FREEZE_BEGIN); } catch (InvalidCastException) { } }
    //public void OnPatrol() { try { if ((IPatrolOrdereable)currentState != null) Send(PlayerInput.PATROL_ORDER); } catch (InvalidCastException) { } }
    //void SendInputReturn()
    //{
    //    bool isidle = false;
    //    try {
    //        if ((State_Idle)currentState != null) Send(PlayerInput.BACK_MY_POS_SUCCESSFUL);
    //        isidle = true;
    //    }
    //    catch (InvalidCastException) { }
    //    if (!isidle) { Send(ispatroller ? PlayerInput.PATROL_ORDER : PlayerInput.BACK_MY_POS_SUCCESSFUL); }
    //}
    //public void SoundInPosition(Vector3 pos)
    //{
    //    try
    //    {
    //        var curr = (ISoundListener)currentState;

    //        if (curr != null)
    //        {
    //            curr.OnSoundListen(pos);
    //            Send(PlayerInput.SOMETHING_WRONG);
    //        }

    //    }
    //    catch (InvalidCastException) { }
    //}
    //public void SoundInPosition(Inspectable inspectable)
    //{
    //    try
    //    {
    //        var curr = (ISoundListener)currentState;

    //        if (curr != null)
    //        {
    //            curr.OnSoundListen(inspectable);
    //            Send(PlayerInput.SOMETHING_WRONG);
    //        }

    //    }
    //    catch (InvalidCastException) { }
    //}

    //#endregion

    //#region Enemy - Entity (Overrides)
    //protected override void OnDeath()
    //{
    //    EV_Death.Invoke();
    //    Send(PlayerInput.DEATH);
    //}
    //protected override void OnLoseLife() { if ((int)life_manager.Life <= (life_manager.GetMax() / 4)) currentState.OnOnCritic(); }
    //public override void ReceiveDamage(int damage, Vector3 destinity, bool isenemy)
    //{
    //    if (!isenemy) life_manager.Hit((int)damage);
    //    else { OnStuned(); }
    //    var vdir = destinity - rb.transform.position;
    //    vdir.y = 0;
    //    vdir.Normalize();
    //    rb.AddForce(vdir * hitforce *-1, ForceMode.Impulse);

    //    ParticlesInstancer.instancia.InstanciarChispas(transform.position);
    //    CompleteCameraController.instancia.Shake();
    //    feedbackEnemy.FeedbackDamage();

    //    OnAttack();
    //}
    
    //protected override void OnGainLife() { }

    //public override void HitThePlayer()
    //{
        
    //}

    //public override void HitTheShield()
    //{
    //    ParticlesInstancer.instancia.InstanciarChispas(transform.position);
    //    CompleteCameraController.instancia.Shake();
    //    feedbackEnemy.FeedbackDamage();
    //    OnStuned();
    //}
    //#endregion

}
