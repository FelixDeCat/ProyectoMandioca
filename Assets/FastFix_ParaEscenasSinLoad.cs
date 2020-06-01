using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFix_ParaEscenasSinLoad : MonoBehaviour
{
    /// <summary>
    /// esto es un fast fix rapido para que los enemigos anden para la clase
    /// esto se va a remplazar por la carga de escenas con carga de scripts 
    /// que estoy armando
    /// </summary>

    private void Start()
    {
        Main.instance.GetNoOptimizedListEnemies().ForEach(x => x.Initialize());
        Main.instance.GetChar().transform.position = Vector3.zero;
    }
    private void Update()
    {
        if (Main.instance.GetChar().transform.position.y < -5)
        {
            Checkpoint_Manager.instance.SpawnChar();
        }
    }
}
