using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillLevelByBranch", menuName = "Skills/LevelByBranch", order = 2)]
public class SkillLevelByBranch : ScriptableObject
{
    public SkillType skilltype;
    
    [Header("Obligatory")]
    public SkillInfo[] Selection;

    [Header("For Random")]
    public bool isFromRandom = true;
    public int max_from_this_branch = 1;
    public int max_from_generics = 2;
}