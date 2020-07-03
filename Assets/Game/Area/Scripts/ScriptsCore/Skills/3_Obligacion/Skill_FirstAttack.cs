using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skill_FirstAttack : SkillBase_Obligacion
{
    public float petrifyRange = 100;
    private CharacterHead _hero;

    CharacterAttack charattack;
    private void Start()
    {
        
    }

    protected override void OnBeginSkill()
    {
        //Main.instance.GetChar().Attack += ReceivePetrifyOnDeathMinion;
        base.OnBeginSkill();
        charattack = Main.instance.GetChar().GetCharacterAttack();
        charattack.ActiveFirstAttack();
        charattack.AddCAllback_ReceiveEntity(RecieveEntity);
    }
    protected override void OnEndSkill()
    {
        base.OnEndSkill();

        //Main.instance.GetChar().Attack -= ReceivePetrifyOnDeathMinion;
        charattack.DeactiveFirstAttack();
        charattack.RemoveCAllback_ReceiveEntity(RecieveEntity);

    }

    protected override void OnUpdateSkill()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            charattack.ActiveFirstAttack();
        }
    }

    public void RecieveEntity()
    {
        //if (charattack.IsFirstAttack())
        //{
        //    foreach (var item in Main.instance.GetEnemies())
        //    {
        //        EnemyBase myEnemy = item.GetComponent<EnemyBase>();

        //        if (myEnemy)
        //        {
        //            myEnemy.OnPetrified();
        //        }
        //    }

        //    charattack.DeactiveFirstAttack();
        //}
        
    }
}
