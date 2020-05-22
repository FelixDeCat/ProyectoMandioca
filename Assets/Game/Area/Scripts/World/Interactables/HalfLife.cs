using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfLife : Interactable
{
    bool execute;
    float timer;

    public int mycant;
    public GenericBar genericbar;

    new public Light light;

    void Start()
    {
        genericbar.Configure(0, mycant, 0.01f);
        light = GetComponentInChildren<Light>();
    }

    public override void OnExecute(WalkingEntity entity)
    {
        execute = true;
    }
    public override void OnExit()
    {
        execute = false;
    }
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(this, "centro de carga", "contiene en total " + mycant + " Cargas", "Cargar");

    }

    void Update()
    {
        //walking entitiy . Heal


        //if (execute)
        //{
        //    if (timer < 1)
        //    {
        //        timer = timer + 1 * Time.deltaTime;

        //        light.color = mycant > 0 ? Color.yellow : Color.red;
        //        light.intensity = Mathf.Lerp(0,2, timer);
        //    }
        //    else
        //    {
        //        light.intensity = 0;

        //        if (CharBrain.instancia.gameloop.EnergyLife.CanHealth())
        //        {
        //            if (mycant > 0)
        //            {
        //                UI_Messages.instancia.ShowMessage("Bip* una celda cargada", 0.5f);
        //                CharBrain.instancia.gameloop.AddEnergy(1);
        //                mycant--;
        //                genericbar.SetValue(mycant);
        //            }
        //            else
        //            {
        //                UI_Messages.instancia.ShowMessage("Bip* Bip* No hay mas cargas", 0.5f);
        //            }
                    
        //        }
        //        else
        //        {
        //            UI_Messages.instancia.ShowMessage("Error de carga* Carga llena", 0.5f);
        //        }
        //        timer = 0;
        //    }
        //}
    }

    public void MensajeTodoCargado()
    {
        
    }
}
