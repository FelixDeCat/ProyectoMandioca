﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatDirector 
{
    Transform CurrentTargetPos();

    Transform CurrentTargetPosDir();

    void SetTargetPosDir(Transform pos);

    void SetTarget(EntityBase entity);

    EntityBase CurrentTarget();

    Vector3 CurrentPos();

    void ToAttack();

    bool IsInPos();

    void SetBool(bool isPos);
}
