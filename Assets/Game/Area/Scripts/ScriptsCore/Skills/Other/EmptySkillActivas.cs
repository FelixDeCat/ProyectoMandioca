using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmptySkillActivas : SkillActivas
{
    protected override void OnBeginSkill() { }
    protected override void OnEndSkill() { }
    protected override void OnOneShotExecute() { Debug.Log("executeeeee"); }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
