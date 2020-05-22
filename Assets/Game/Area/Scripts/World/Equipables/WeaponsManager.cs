using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using w = Weapons;

public class WeaponsManager : ManagerEquipableBase
{
    [Header("WeaponsManager")]
    public Item basicweapon;

    public int strength = 1;

   // public TrailHandlerFeedback charfeedback;
   // public CharAnim anim;
    public SensorManagerForWeapons sensorManager;

    public w.Weapon current_weapon;

    private void Start()
    {
        sensorManager.SetCallback(OnHit);
       // Debug.Log("Equipando..." + basicweapon.name);
        EquipItem(basicweapon);
    }

    protected override void OnItemEquiped(Equipable equipable)
    {
        current_weapon = (w.Weapon)equipable;
       // charfeedback.SetCurrentTrail(current_weapon.mytrail);
       // sensorManager.EquipSensor(current_weapon.SensorType);
        sensorManager.Off();
    }

    public void OnHit(GameObject go)
    {
        var damageable = go.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
         //  Combos.instance.ReceiveHit(3);
            damageable.ReceiveDamage(strength + current_weapon.damage, this.transform.position);
            current_weapon.Hit(go);
        }
    }

    public void BeginAttack()
    {
    //    anim.ShootTriggerAttack1();
    //    charfeedback.OnBeginAttack();
    }

    // tiene animacion con trigger, dispara a ConfirmAttack()
    public void ConfirmAttack() { current_weapon.BeginUse(); sensorManager.On(); }
    public void CancelAttack() { current_weapon.EndUse(); sensorManager.Off(); }
    public void EndAnimation() { /*charfeedback.OnEndAttack();*/ sensorManager.Off(); }

    public void EndAttack() { }


    private void Update()
    {
        if (current_weapon != null)
            current_weapon.UpdateUse();
    }

   
}
