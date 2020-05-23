using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCube : EntityBase
{
    Rigidbody myRig;
    private void Awake() => myRig = GetComponent<Rigidbody>();
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype)  
    {
        Invoke("Destroy", 0f);
        var dir = (this.transform.position - attack_pos).normalized; 
        myRig.AddForce(dir * 10, ForceMode.VelocityChange); 
        return Attack_Result.inmune; 
    }

    public void Destroy()
    {
        PoolManager.instance.ReturnObject(this);
    }
    protected override void OnFixedUpdate() { }
    protected override void OnInitialize() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdate() { }
    protected override void OnTurnOff() { }
}
