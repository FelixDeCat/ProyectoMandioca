using UnityEngine;

public class InteractableTeleport : Interactable
{
    [Header("Teleport Settings")]
    public string titulo = "Teleport";
    [Multiline(10)]
    public string informacion_del_teleport;
    public bool mostrar_cartelito = true;
    public Transform transform_destino;
    public override void OnExecute(WalkingEntity entity) => Main.instance.GetChar().transform.position = transform_destino.position;
    public override void OnExit() => WorldItemInfo.instance.Hide();
    public override void OnEnter(WalkingEntity entity)
    {
        if (mostrar_cartelito)
        {
            if(pointToMessage != null) WorldItemInfo.instance.Show(pointToMessage.transform.position, titulo, informacion_del_teleport, "Entrar");
            else WorldItemInfo.instance.Show(this.transform.position, titulo, informacion_del_teleport, "Entrar");
        }
    }
}
