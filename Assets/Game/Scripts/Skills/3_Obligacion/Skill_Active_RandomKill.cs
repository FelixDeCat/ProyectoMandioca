using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Active_RandomKill : SkillActivas
{
    List<EnemyBase> _myEnemys = new List<EnemyBase>();
    CharacterHead _player;
    [SerializeField] int _playerDMGReceive = 10;
    protected override void OnBeginSkill()
    {
        _player = Main.instance.GetChar();
        SetPredicate(CanUseChange);
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
        Debug.Log("Instakill");
        _myEnemys = Main.instance.GetNoOptimizedListEnemies();
        if (_myEnemys.Count > 0)
        {
            var charblock = (CharacterBlock)_player.GetCharBlock();

            int index = Random.Range(0, _myEnemys.Count);
            _myEnemys[index].TakeDamage(200, transform.position, Damagetype.normal, Main.instance.GetChar());
            _player.TakeDamage(_playerDMGReceive, _player.transform.position, Damagetype.normal);
            _player.transform.position = _myEnemys[index].transform.position;//solo para testing borrar

            charblock.SetBlockCharges(-3);
        }
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
