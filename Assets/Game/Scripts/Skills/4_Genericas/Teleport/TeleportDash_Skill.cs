using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDash_Skill : SkillBase
{
    private CharacterHead _hero;
    [SerializeField] private float teleportDistance = 6;
    private CharacterMovement _movement;

    private bool teleportEnabled = false;
    protected override void OnBeginSkill()
    {
        if(_hero == null)
            _hero = FindObjectOfType<CharacterHead>();

        if (_movement == null)
            _movement = _hero.GetCharMove();
        
        _hero.ChangeDashForTeleport();
        
        _movement.ConfigureTeleport(teleportDistance);

        teleportEnabled = true;
    }

    protected override void OnEndSkill()
    {
        _hero.ChangeTeleportForDash();
        
        teleportEnabled = false;
    }

    protected override void OnUpdateSkill()
    {
        
    }

    private void OnDrawGizmos()
    {
        
        if(!teleportEnabled)
            return;
        
        
        Vector3 lookDir = _movement.GetLookDirection();
        Vector3 heroPos = _hero.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(heroPos + (lookDir * teleportDistance), .6f);
        
        
    }
}
