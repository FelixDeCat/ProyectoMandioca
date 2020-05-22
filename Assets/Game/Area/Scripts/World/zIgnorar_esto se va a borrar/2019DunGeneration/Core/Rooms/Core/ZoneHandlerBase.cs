using UnityEngine;

public abstract class ZoneHandlerBase : MonoBehaviour
{
    public static ZoneHandlerBase instancia;
    private void Awake() => instancia = this;

    public ZoneBase currentZone;
    public void SetCurrentZone(ZoneBase room) => currentZone = room;
    public ZoneBase GetCurrentZone() => currentZone;
}
