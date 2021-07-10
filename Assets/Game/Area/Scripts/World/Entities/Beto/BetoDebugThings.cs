using UnityEngine;
public class BetoDebugThings : MonoBehaviour
{
    BetoBoss beto;
    public static BetoDebugThings instance;
    private void Awake() => instance = this;
    private void Start() => beto = GetComponent<BetoBoss>();
    public static void Instant_Kill_Beto() => instance.KillBeto(); //por si quisieramos llamarlo desde otra escena
    public void KillBeto() => beto.DEBUG_InstaKill();
}
