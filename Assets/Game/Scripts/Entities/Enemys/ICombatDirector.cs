using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatDirector 
{
    //Vector3 CurrentTargetPos();

    //Transform CurrentTargetPosDir();

    //void SetTargetPosDir(Transform pos);

    //float GetDistance();

    void SetTarget(EntityBase entity);

    EntityBase CurrentTarget();

    Vector3 CurrentPos();

    void ToAttack();

    bool IsInPos();

    void SetBool(bool isPos);

    void ResetCombat();
}
