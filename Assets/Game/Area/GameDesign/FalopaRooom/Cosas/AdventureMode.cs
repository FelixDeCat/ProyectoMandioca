using System.Collections;
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
        _hero.UpWeaponsAction += CombatModeSpeed;
        _hero.DownWeaponsAction += AdventureModeSpeed;
    }

    void AdventureModeSpeed()
    {
        Debug.Log("Velocidad de aventura");
        _hero.GetCharMove().SetSpeed(startingSpeed * speedScaler);
    }

    void CombatModeSpeed()
    {
        Debug.Log("Velocidad de combate");
        _hero.GetCharMove().SetSpeed();
    }
}
