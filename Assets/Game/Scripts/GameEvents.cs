using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pongamos los Summary porque nos vamos a volver locos si no recordamos 
// que era cada parametro, asi desde el otro lado solo con posar el 
// mouse vemos y no tenemos que andar navegando para buscar que se triggerea

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
    /// [ Minion : ref del Minion ]
    /// </summary>
    public const string MINION_SPAWN = "MinionSpawn";

    public const string GAME_END_LOAD = "GameEndLoad";
    public const string GAME_INITIALIZE = "GameInitialize";
}
