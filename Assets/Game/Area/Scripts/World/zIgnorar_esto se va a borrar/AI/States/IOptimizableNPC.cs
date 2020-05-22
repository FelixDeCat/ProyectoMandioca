using UnityEngine;
public interface IOptimizableNPC
{
    void OnPlayerActiveMe(Transform who);
    void OnPlayerDeactiveMe();
}