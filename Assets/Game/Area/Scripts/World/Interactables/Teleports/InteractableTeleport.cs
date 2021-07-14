using UnityEngine;
using System;
using UnityEngine.Events;

public class InteractableTeleport : Interactable
{
    public enum TeleportType { change_posicional, change_scene }
    [Header("--- Teleport Settings ---")]
    public TeleportType teleportType;
    public string titulo = "Teleport";
    public string interactInfo = "Entrar";
    [Multiline(10)]
    public string informacion_del_teleport = "bla bla bla";
    public bool mostrar_cartelito = true;
    public bool mostrar_Descripcion = true;
    public Transform transform_destino;
    public string sceneToChange;
    public bool UseLocalSceneStreamer;

    public bool show_permanentSign;

    bool Oneshot;

    private void Start()
    {
        if (show_permanentSign)
        {
            WorldItemInfo.instance.Show(this.transform.position, titulo, informacion_del_teleport, interactInfo, false, mostrar_Descripcion);
        }
    }

    public override void OnExecute(WalkingEntity entity)
    {
        if (!Oneshot)
        {
            Oneshot = true;
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
                   // Fades_Screens.instance.FadeOff(() => { });
                    LoadSceneHandler.instance.On_LoadScreen();
                    GameLoop.instance.StopGame();
                    NewSceneStreamer.GotToAntiBugPosition();
                    NewSceneStreamer.instance.LoadScene(sceneToChange, EndLoad);
                    AudioManager.instance.Mute();
                }
            }
            if (teleportType == TeleportType.change_scene) Fades_Screens.instance.FadeOn(On_FadeOn_Ended);

            ReturnToCanExecute();
        }
    }
    float timer;
    protected override void Update()
    {
        base.Update();

        if (Oneshot)
        {
            if (timer < 1)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                Oneshot = false;
            }
        }
    }
    void EndLoad()
    {
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();
        GameLoop.instance.StartGame();
        Main.instance.GetChar().GetCharMove().StopDamageFall();
        AudioManager.instance.Unmute();
        Main.instance.GetChar().transform.position = transform_destino.position;
    }

    public void On_FadeOn_Ended()
    {
        Scenes.Load(sceneToChange);
    }

    public override void OnInterrupt()
    {
    }

    public override void OnExit(WalkingEntity collector) { WorldItemInfo.instance.Hide(); Oneshot = false; } 
    public override void OnEnter(WalkingEntity entity)
    {
        if (mostrar_cartelito)
        {
            Main.instance.GetChar().GetCharMove().StopDamageFall();
            if (pointToMessage != null) WorldItemInfo.instance.Show(pointToMessage.transform.position, titulo, informacion_del_teleport, interactInfo, false, mostrar_Descripcion);
            else WorldItemInfo.instance.Show(this.transform.position, titulo, informacion_del_teleport, interactInfo, false, mostrar_Descripcion);
        }
    }

    private void OnDrawGizmos()
    {
        if (transform_destino != null)
        {
            Gizmos.DrawLine(transform.position + (Vector3.up * 0.5f), transform_destino.position);
        }
    }
}
