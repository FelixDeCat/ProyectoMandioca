using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HalfLife : SkillBase_Obligacion
{
    CharacterHead character;

    private void Start()
    {
        character = Main.instance.GetChar();
    }
    protected override void OnBeginSkill()
    {
        base.OnBeginSkill();
        //var enemysInRoom = Main.instance.GetRoom().myenemies();
        //for (int i = 0; i < enemysInRoom.Count; i++)
        //{
        //    enemysInRoom[i].HalfLife();

        //}
        for (int i = 0; i < myEnemies.Count; i++)
        {
            myEnemies[i].HalfLife();
        }
    }

    protected override void OnEndSkill()
    {
        base.OnEndSkill();
        var enemysInRoom = Main.instance.GetRoom().GetMyEnemies();
        for (int i = 0; i < enemysInRoom.Count; i++)
        {
            enemysInRoom[i].Mortal();

        }
    }

    protected override void OnUpdateSkill()
    {

    }

    
}
