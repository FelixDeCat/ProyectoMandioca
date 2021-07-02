/////////////////////////////////////////////////////////////////////////////////////////////////////
// pongamos los Summary si son muchos parametros porque nos vamos a volver locos si no recordamos 
// que era cada parametro, asi desde el otro lado solo con posar el 
// mouse vemos y no tenemos que andar navegando para buscar que se triggerea
/////////////////////////////////////////////////////////////////////////////////////////////////////

public class GameEvents
{
    /// <summary>
    /// [ vector3: posicion ]
    /// [ boolean: isPetrified ]
    /// [ int: cantExpToDrop ]
    /// </summary>
    public const string ENEMY_DEAD = "EnemyDead";
    /// <summary>
    /// [ EnemyBase : ref del enemigo ]
    /// </summary>
    public const string ENEMY_SPAWN = "EnemySpawn";


    /// <summary>
    /// [ EntityBase: target parreado ]
    /// </summary>
    public const string ON_PLAYER_PARRY = "OnPlayerParry";
    public const string ON_PLAYER_DEATH = "CharacterIsDeath";
    public const string ON_PLAYER_RESPAWN = "OnPlayerRespawn";
    public const string GAME_END_LOAD = "GameEndLoad";
    public const string COMBAT_ENTER = "CombatEnter";
    public const string COMBAT_EXIT = "CombatExit";
    public const string GAME_INITIALIZE = "GameInitialize";
    public const string INTERACTABLES_INITIALIZE = "InteractablesInitialize";
    public const string DELETE_INTERACTABLE = "DeleteInteractable";
    public const string ADD_INTERACTABLE = "AddInteractable";
    public const string CARONTE_DEFEAT_IN_JOJO_DUNGEON = "CaronteDefeat";

    public const string CARONTE_START = "CaronteStart";
    public const string CARONTE_RESET = "CaronteReset";
    public const string BETO_START = "BetoStart";
    public const string BETO_RESET = "BetoReset";
    public const string BETO_DEFEATED = "BetoDefeat";
    public const string ADD_PLANT = "AddPlant";
    public const string ADD_ENT_DEAD_WITH_ROCK = "AddEntDeadWithRock";
    public const string END_GAME = "EndGame";

    public const string CHANGE_INPUT = "ChangeInput";
}
