using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class BoomerangExplosion : MonoBehaviour
{
    [SerializeField] private GameObject auxShield = null;

    [SerializeField] int damage = 10;
    [SerializeField] float damageRadius = 3;
    [SerializeField] Damagetype damagetype;

    CharacterHead _hero;
    GameObject _shield;
    Transform auxParent = null;
    bool shortCast = false;
    
    DamageData dmgData = null;

    [SerializeField] float minChargeTime = 0.2f;
    [SerializeField] float maxChargeTime = 2f;

    float timer = 0;
    bool canUpdate = false;

    protected void Start()
    {
        auxShield.SetActive(false);
    }

    ///////////////////////////////////////
    //  USE
    ///////////////////////////////////////
    public void OnPress()
    {
        _hero.ChargeThrowShield();

        timer = 0;
        canUpdate = true;
    }

    public void OnStopUse()
    {

    }

    ///////////////////////////////////////
    //  EQUIP
    ///////////////////////////////////////
    public void Equip()
    {
        _hero = Main.instance.GetChar();
        _shield = _hero.escudo;
        dmgData = auxShield.GetComponent<DamageData>();
        dmgData.Initialize(_hero);
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(damagetype).SetKnockback(0).SetPositionAndDirection(_shield.transform.position);

        auxParent = auxShield.transform.parent;

    }
    public void UnEQuip()
    {

    }

    ///////////////////////////////////////
    //  EXECUTE SKILL
    ///////////////////////////////////////
    ///
    public void OnUpdate()
    {
        if (!canUpdate) return;
        if (timer < maxChargeTime)
        {
            timer += Time.deltaTime;
        }
    }

    public void OnExecute(int charges)
    {
        if (charges == 0) ExecuteShort();
        else ExecuteLong();
    }

    void ExecuteShort()
    {
        Debug.Log("Por ahora no hace nada");
    }

    void ExecuteLong()
    {

    }

    public void DealDamageNearbyEnemies()
    {
        var enemiesClose = Extensions.FindInRadius<DamageReceiver>(auxShield.transform.position, damageRadius);

        foreach (DamageReceiver enemy in enemiesClose)
        {
            if (enemy.GetComponent<EntityBase>() != _hero)
                enemy.TakeDamage(dmgData);
        }
    }
}
