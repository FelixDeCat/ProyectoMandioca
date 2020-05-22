using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_Invisible : SkillActivas
{
    [SerializeField] private float duration;
    
    
    protected override void OnOneShotExecute() { }
    protected override void OnBeginSkill() { }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
