public enum Attack_Result { sucessful, blocked, parried, reflexed, inmune, death }
public enum side_type { neutral, ally, enemy, other }
[System.Serializable] public enum SkillType { control, culpa, obligacion, generics, others }
public enum Damagetype { Fire, normal, parriable, explosion, inparry, heavy }
public enum CommonStates { IDLE, ATTACK, ENABLE, DISABLE, DIE, CHASING, GO_TO_POS }
public enum AudioGroups {GAME_FX, MUSIC, MISC, JABALI, SLOWMO}

public enum LabelStatesLinkType
{
    STATE_DEACTIVATED,
    STATE_INTRO,
    STATE_IDLE,
    STATE_FOLLOW,

    STATE_CHANGE_FASE_1,
    STATE_CHANGE_FASE_2,
    STATE_CHANGE_FASE_3,

    // esto por ahora es pura especulacion
    // pero lo dejo xq es para darme una idea de las posiciones
    // todavia no tengo redondeada la idea final, cuando lo haga borro lo que no sirve (y)

    STATE_01_ACTION,
    STATE_02_ACTION,
    STATE_03_ACTION,

    STATE_11_BEGIN_MELEE,
    STATE_12_CALCULATE_MELEE,
    STATE_13_MELEE_FAST,
    STATE_14_MELEE_STRONG,
    STATE_15_MELEE_COMBO,

    STATE_21_BEGIN_RANGE,
    STATE_22_CALCULATE_RANGE,
    STATE_23_RANGE_FAST,
    STATE_24_RANGE_STRONG,
    STATE_25_RANGE_COMBO,

    STATE_TAUNT_1,
    STATE_TAUNT_2,

    STATE_DEATH,

    STATE_FIND

}

//////////
// UI stuff

public enum UI_templates {skillSelection, charStats};

public enum soundTypes {golpeEspada, heroReceiveDamage}
