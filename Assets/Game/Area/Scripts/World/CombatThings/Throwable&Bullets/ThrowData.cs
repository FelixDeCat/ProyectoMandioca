using UnityEngine;
public class ThrowData
{
    #region local
    Vector3 position; 
    Vector3 vectorDirection; 
    float external_force; 
    int damage;
    Transform owner;
    #endregion
    internal Vector3 Position => position;
    internal Vector3 Direction => vectorDirection;
    internal float Force => external_force;
    internal int Damage => damage;
    internal Transform Owner => owner;
    internal ThrowData Configure(Vector3 _position, Vector3 _direction, float _force, int _damage, Transform _owner)
    {
        position = _position;
        vectorDirection = _direction;
        external_force = _force;
        damage = _damage;
        owner = _owner;
        return this;
    }
}
