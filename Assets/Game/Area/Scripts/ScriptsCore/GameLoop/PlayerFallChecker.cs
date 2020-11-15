using UnityEngine;
using UnityEngine.Events;

public class PlayerFallChecker : MonoBehaviour
{
    public float Position_To_Death = -100;
    Transform charTransform;
    bool startgame;
    public UnityEvent OnPlayerDeath;
    public void StartGame() { startgame = true; charTransform = Main.instance.GetChar().transform; }
    public void StopGame() { startgame = false; }

    private void Update()
    {
        if (!startgame) return;
        if (charTransform.position.y < Position_To_Death)
        {
            OnPlayerDeath.Invoke();
        }
    }
}
