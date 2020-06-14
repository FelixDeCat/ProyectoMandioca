using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFix_ParaEscenasSinLoad : MonoBehaviour
{
    public bool hasSpawn;
    /// <summary>
    /// esto es un fast fix rapido para que los enemigos anden para la clase
    /// esto se va a remplazar por la carga de escenas con carga de scripts 
    /// que estoy armando
    /// </summary>

    private void Start()
    {
        if (Main.instance == null)
        {
            Scenes.Load_0_Load();
            return;
        }

        Main.instance.GetNoOptimizedListEnemies().ForEach(x => x.Initialize());
        Main.instance.GetNoOptimizedDestructibles().ForEach(x => x.Initialize());     

        if (hasSpawn)
        {
            Checkpoint_Manager.instance.SpawnChar();
            //Main.instance.GetChar().transform.position = this.transform.position;
        }
        else
        {
            Main.instance.GetChar().transform.position = Vector3.zero;
        }
    }
    private void Update()
    {
    }
}
