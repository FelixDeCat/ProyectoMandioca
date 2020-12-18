using UnityEngine;
using TMPro;
public class EnemyExample : MonoBehaviour, StunManager.IStuned
{
    public TextMeshPro ui;
    int seconds;
    public int GetSeconds() { return seconds; }
    public void Ready() { if (ui) ui.text = ""; }
    public void SetSeconds(int seconds)
    {
        this.seconds = seconds;
        if (ui) ui.text = seconds > 0 ? "-" + seconds.ToString() : "►";
    }
}
