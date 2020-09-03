using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatDirector 
{
    void SetTarget(EntityBase entity);
    float GetDistance();
    EntityBase CurrentTarget();
    Vector3 CurrentPos();
    void ToAttack();
    bool IsInPos();
    void SetBool(bool isPos);
    void ResetCombat();
}
