﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureMode : MonoBehaviour
{
    CharacterHead _hero;

    [SerializeField] float speedScaler = 1.3f;
    float startingSpeed;

    private void Start()
    {
        _hero = GetComponent<CharacterHead>();
        startingSpeed = _hero.GetCharMove().GetDefaultSpeed;
        RegisterEvents();
    }

    void AdventureModeSpeed()
    {
        _hero.GetCharMove().SetSpeed(startingSpeed * speedScaler);
    }

    void CombatModeSpeed()
    {
        _hero.GetCharMove().SetSpeed();
    }

    public void UnregisterEvents()
    {
        _hero.UpWeaponsAction -= CombatModeSpeed;
        _hero.DownWeaponsAction -= AdventureModeSpeed;
    }

    public void RegisterEvents()
    {
        _hero.UpWeaponsAction += CombatModeSpeed;
        _hero.DownWeaponsAction += AdventureModeSpeed;
    }
}
