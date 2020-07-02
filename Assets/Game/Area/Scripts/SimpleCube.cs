using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCube : EntityBase
{
    Rigidbody myRig;
    private void Awake() => myRig = GetComponent<Rigidbody>();
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
