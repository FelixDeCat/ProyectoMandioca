using UnityEngine;
using UnityEngine.Events;

public class PlayerFallChecker : MonoBehaviour
{
    public float Position_To_Death = -100;
    Transform charTransform;
    bool startgame;
    public UnityEvent OnPlayerDeath;
    bool execute;
    public void StartGame() { startgame = true; charTransform = Main.instance.GetChar().transform; }
    public void StopGame() { startgame = false; }

    void OnReset()
    {
        execute = true;
    }

    private void Update()
    {
        if (!execute) return;
        if (!startgame) return;
        if (charTransform.position.y < Position_To_Death)
        {
            OnPlayerDeath.Invoke();
            execute = false;
            Invoke("OnReset", 2f);
        }
    }
}
