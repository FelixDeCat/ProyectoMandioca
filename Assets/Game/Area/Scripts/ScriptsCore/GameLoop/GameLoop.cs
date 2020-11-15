using UnityEngine;
using UnityEngine.Events;
public class GameLoop : MonoBehaviour
{
    public static GameLoop instance; private void Awake() => instance = this;
    [Header("GameLoop Events")]
    public UnityEvent UE_OnStartGame;
    public UnityEvent UE_OnStopGame;
    [Header("Transition Events")]
    public UnityEvent UE_OnPlayerDeath;
    public UnityEvent UE_OnTeleport;
    void Start() => Main.instance.GetChar().Life.ADD_EVENT_Death(OnPlayerDeath);
    public void StartGame() { UE_OnStartGame.Invoke(); }
    public void BehindTeleportCheking() => UE_OnTeleport.Invoke();
    public void StopGame() => UE_OnStopGame.Invoke();
    public void OnPlayerDeath() => UE_OnPlayerDeath.Invoke();

}
