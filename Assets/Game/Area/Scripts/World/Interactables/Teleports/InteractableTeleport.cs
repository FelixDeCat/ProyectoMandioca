using UnityEngine;

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
    public Transform transform_destino;
    public string sceneToChange;
    public override void OnExecute(WalkingEntity entity) 
    { 
        if(teleportType == TeleportType.change_posicional) Main.instance.GetChar().transform.position = transform_destino.position;
        if (teleportType == TeleportType.change_scene) Fades_Screens.instance.FadeOn(On_FadeOn_Ended);
    }

    public void On_FadeOn_Ended()
    {
        Scenes.Load(sceneToChange);
    }

    public override void OnExit() => WorldItemInfo.instance.Hide();
    public override void OnEnter(WalkingEntity entity)
    {
        if (mostrar_cartelito)
        {
            if(pointToMessage != null) WorldItemInfo.instance.Show(pointToMessage.transform.position, titulo, informacion_del_teleport, interactInfo, false, false);
            else WorldItemInfo.instance.Show(this.transform.position, titulo, informacion_del_teleport, interactInfo, false, false);
        }
    }
}
