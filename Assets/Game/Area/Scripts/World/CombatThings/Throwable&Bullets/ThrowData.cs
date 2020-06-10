using UnityEngine;
public class ThrowData
{
    #region local
    Vector3 position; 
    Vector3 vectorDirection; 
    float external_force; 
    int damage;
    #endregion
    internal Vector3 Position => position;
    internal Vector3 Direction => vectorDirection;
    internal float Force => external_force;
    internal int Damage => damage;
    internal void Configure(Vector3 _position, Vector3 _direction, float _force, int _damage)
    {
        position = _position;
        vectorDirection = _direction;
        external_force = _force;
        damage = _damage;
    }
}
