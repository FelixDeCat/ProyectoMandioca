using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelElectrico //: Interactable, IRoomElementable
{
    //public PanelInspectable panel;

    //public GenericBar genbar;
    //public Text textfeedback;
    //public Image img_cake;
    //public Transform point_explosion;

    //public int radio_explosion;
    //public float time_to_explode;
    //float timer;
    //bool go_to_explode;
    //bool oneshot;

    //public NewRoom room;
    //public RoomBase manual_room;

    //private void Awake()
    //{
    //    genbar.Configure(0, (int)time_to_explode, 0.01f);
    //}

    //public override void Execute()
    //{
    //    if (!oneshot)
    //    {
    //        go_to_explode = true;
    //        timer = time_to_explode;
    //        oneshot = true;
    //    }
    //}

    //private void Update()
    //{
        

    //    if (go_to_explode)
    //    {
    //        if (timer > 0)
    //        {
    //            textfeedback.text = ((int)timer).ToString();
    //            timer = timer - 1 * Time.deltaTime;
    //            genbar.SetValueCake(timer);
    //        }
    //        else
    //        {
    //            go_to_explode = false;
    //            timer = time_to_explode;
    //            panel.UnMarkAsInspected();
    //            panel.MarkSomethingWrong();
    //            ParticlesInstancer.instancia.InstanciarExplosionAzul(point_explosion.position);
    //            img_cake.enabled = false;
    //            textfeedback.enabled = false;
    //            ParticlesInstancer.instancia.InstanciarSonido(point_explosion.position,radio_explosion);
    //            SoundSystem.instancia.GenerateSoundWave(panel,radio_explosion, manual_room);
    //        }
    //    }
    //}

    

    //public override void Exit()
    //{
        
    //}

    //public override void ShowInfo()
    //{
    //    WorldItemInfo.instance.Show(this.transform.position,
    //                                "Panel Electrico",
    //                                "Modifica el panel electrico para que explote en "+ (time_to_explode-1) +" segundos",
    //                                "dañar");
    //}

    //public void SetRoom(NewRoom newroom) => room = newroom;
    //public void SetmanualRoom(RoomBase newroom) => manual_room = newroom;
}
