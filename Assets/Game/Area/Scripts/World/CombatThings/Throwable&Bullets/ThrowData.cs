using System;
using UnityEngine;

[Serializable]
public class ThrowData
{
    public Vector3 Position;
    public Vector3 Direction;
    public float Force;
    public int Damage;
    public Transform Owner;
    public Action<Vector3> OnHitFloor_callback;
    internal ThrowData Configure(Vector3 _position, Vector3 _direction, float _force, int _damage, Transform _owner, Action<Vector3> onHitfloor_callback = default)
    {
        Position = _position;
        Direction = _direction;
        Force = _force;
        Damage = _damage;
        Owner = _owner;
        OnHitFloor_callback = onHitfloor_callback;
        return this;
    }
}
