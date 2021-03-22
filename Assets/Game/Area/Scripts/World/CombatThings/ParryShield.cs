using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryShield : MonoBehaviour
{
    [SerializeField] DamageReceiver dmgReceiver = null;

    private void Awake()
    {
        dmgReceiver.SetParry((x, y, z) => true, ParryFeedback).Initialize(transform, null, null);
    }

    void ParryFeedback(EntityBase _entity)
    {

    }
}
