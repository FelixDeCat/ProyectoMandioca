﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : Interactable
{
    public Transform spawnposition;

    public Vector3 GetPosition() => spawnposition.position;

    public override void OnEnter(WalkingEntity entity) { }
    public override void OnExecute(WalkingEntity collector) { }
    public override void OnExit() { }
}
