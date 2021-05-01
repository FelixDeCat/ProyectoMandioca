using UnityEngine;
using System;
using UnityEngine.Events;

public class InteractableTeleport : Interactable
{
    public enum TeleportType { change_posicional, change_scene }
    public TeleportType teleportType;
    [Header("Teleport Settings")]
    public string titulo = "Teleport";
    public string interactInfo = "Entrar";
    [Multiline(10)]
    public string informacion_del_teleport = "bla bla bla";
    public bool mostrar_cartelito = true;
    public bool mostrar_Descripcion = true;
    public Transform transform_destino;
    public string sceneToChange;
    public bool UseLocalSceneStreamer;

    
    public override void OnExecute(WalkingEntity entity)
    {
        if (teleportType == TeleportType.change_posicional)
        {

            if (!UseLocalSceneStreamer)
            {
                Main.instance.GetChar().GetCharMove().StopDamageFall();
                Main.instance.GetChar().transform.position = transform_destino.position;
            }
            else
            {
                Fades_Screens.instance.Black();
                Fades_Screens.instance.FadeOff(() => { });
                LoadSceneHandler.instance.On_LoadScreen();
                GameLoop.instance.StopGame();
                NewSceneStreamer.instance.LoadScene(sceneToChange, EndLoad);
            }
        }
        if (teleportType == TeleportType.change_scene) Fades_Screens.instance.FadeOn(On_FadeOn_Ended);

        ReturnToCanExecute();
    }
    void EndLoad()
    {
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();
        GameLoop.instance.StartGame();
        Main.instance.GetChar().GetCharMove().StopDamageFall();
        Main.instance.GetChar().transform.position = transform_destino.position;
    }

    public void On_FadeOn_Ended()
    {
        Scenes.Load(sceneToChange);
    }

    public override void OnInterrupt()
    {
    }

    public override void OnExit(WalkingEntity collector) => WorldItemInfo.instance.Hide();
    public override void OnEnter(WalkingEntity entity)
    {
        if (mostrar_cartelito)
        {
            Main.instance.GetChar().GetCharMove().StopDamageFall();
            if (pointToMessage != null) WorldItemInfo.instance.Show(pointToMessage.transform.position, titulo, informacion_del_teleport, interactInfo, false, mostrar_Descripcion);
            else WorldItemInfo.instance.Show(this.transform.position, titulo, informacion_del_teleport, interactInfo, false, mostrar_Descripcion);
        }
    }
}
