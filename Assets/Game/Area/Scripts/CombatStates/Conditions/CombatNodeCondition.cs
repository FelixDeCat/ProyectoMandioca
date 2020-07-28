using UnityEngine;
using System;

/////////////////////////////////////////////////////////////
/*
    Existen 2 formas de disparar la condicion
    - a traves de una Funcion ejecutada desde afuera que ejecuta un callback configurable tambien desde afuera
    - Un predicado por update que retorna un bool en el caso de que tengamos que hacer checkeos continuos
*/
/////////////////////////////////////////////////////////////
public abstract class CombatNodeCondition : MonoBehaviour 
{
    public void Init() => OnInit();
    protected abstract void OnInit();

    // FORMA 1... por Funcion, a mano
    Action ManualShoot = delegate { };
    public void Init(Action CallbakcManualShoot) { ManualShoot = CallbakcManualShoot; OnInit(); }
    public void UEVENT_ShootCondition() => ManualShoot.Invoke(); 
    
    // FORMA 2... por predicado
    public abstract bool RefreshPredicate();
}
