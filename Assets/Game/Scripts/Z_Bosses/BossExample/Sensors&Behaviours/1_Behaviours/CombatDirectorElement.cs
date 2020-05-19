using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirectorElement : MonoBehaviour, ICombatDirector
{
    [Header("TEMP:/Combat director")]
    [SerializeField, Range(0.5f, 15)] float distancePos = 1.5f;
    bool withPos;

    CombatDirector director;
    EntityBase owner;
    EntityBase target;
    bool combat;

    void SubscribeMeToInitializer()
    {
        //aca me subscribo a los sensores de inicializacion

        //tanto para el del proximidad
        //como el del habitacion
    }

    public void Ev_OutOfDistance()
    {
        director.DeadEntity(this, target);
        target = null;
        combat = false;
    }

    public void Initialize(CombatDirector _director, EntityBase entityBase)
    {
        director = _director;
        director.AddNewTarget(entityBase);
    }

    public Vector3 CurrentPos() => owner.transform.position;
    public void SetTarget(EntityBase entity) { target = entity; }
    public bool IsInPos() => withPos;
    public EntityBase CurrentTarget() => target;
    public void SetBool(bool isPos) => withPos = isPos;
    public void ResetCombat()
    {
        target = null;
        combat = false;
        SetBool(false);
    }
    public void ToAttack() { }

}
   
