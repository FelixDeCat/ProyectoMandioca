using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelSystem/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int level = 0;
    public int maxt_to_level_up = 10;
    public bool can_get_skill_point = true;
}
