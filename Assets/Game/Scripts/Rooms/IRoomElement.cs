using UnityEngine;

public interface IZoneElement
{
    void Zone_OnDungeonGenerationFinallized();
    void Zone_OnPlayerEnterInThisRoom(Transform who);
    void Zone_OnPlayerExitInThisRoom();
    void Zone_OnUpdateInThisRoom();
    void Zone_OnPlayerDeath();
}
