using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSwordShield : TestInteractableHold
{
    CharacterHead character;
    [SerializeField] bool isSword;


    public override void OnExecute(WalkingEntity collector)
    {
        base.OnExecute(collector);
        character = collector.gameObject.GetComponent<CharacterHead>();

        if (character && isSword)
        {
            character.ToggleAttack(true);
            character.currentWeapon.SetActive(true);
        }
        else if (character)
        {
            character.ToggleBlock(true);
            character.charBlock.shield.SetActive(true);
        }

    }

}
