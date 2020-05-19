using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skill_Active_RandomKill : SkillActivas
{
    List<EnemyBase> _myEnemys = new List<EnemyBase>();
    CharacterHead _player;
    [SerializeField] int _playerDMGReceive = 10;

    [SerializeField] Atenea atenea;

    protected override void OnBeginSkill()
    {
        _player = Main.instance.GetChar();
        SetPredicate(CanUseChange);

        var aux = atenea.GetComponent<AnimEvent>();
        aux.Add_Callback("SelectEnemy", SelectEnemy);
        aux.Add_Callback("TiraRayo", KillEnemy);
    }

    EnemyBase enemSelected;

    void SelectEnemy()
    {
        _myEnemys = Main.instance.GetNoOptimizedListEnemies();

        if (_myEnemys.Count > 0)
        {
            int index = Random.Range(0, _myEnemys.Count);
            enemSelected = _myEnemys[index];
        }

        if (enemSelected != null)
        {
            var dir = atenea.transform.position - enemSelected.transform.position;
            dir.Normalize();
            atenea.transform.forward = dir;
        }
    }
    void KillEnemy()
    {
        if (enemSelected != null)
        {
            var charblock = (CharacterBlock)_player.GetCharBlock();
            enemSelected.TakeDamage(200, transform.position, Damagetype.normal, Main.instance.GetChar());
            charblock.SetBlockCharges(-3);
        }
    }


    protected override void OnEndSkill()
    {
    }

    bool CanUseChange()
    {
        var charblock = (CharacterBlock)_player.GetCharBlock();
        return charblock.CanUseCharge();
    }

    protected override void OnOneShotExecute()
    {
        atenea.gameObject.SetActive(true);
        atenea.GoToHero();
        atenea.Anim_SmiteBegin();
    }

    protected override void OnStartUse()
    {
    }

    protected override void OnStopUse()
    {
    }

    protected override void OnUpdateSkill()
    {
    }

    protected override void OnUpdateUse()
    {
    }

   
}
