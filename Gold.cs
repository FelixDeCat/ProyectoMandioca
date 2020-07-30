using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : StatBase
{
    public override void CanNotAddMore() { }
    public override void CanNotRemoveMore() { }
    public override void OnAdd() { }
    public override void OnLoseAll() { }
    public override void OnRemove() { }
    public override void OnValueChange(int value, int max, string message) { }
}
