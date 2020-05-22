using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDestructibleData", menuName = "DestructibleData", order = 10)]
public class DestructibleData : ScriptableObject
{
    public int minDrop, maxDrop;
    public List<Stats> data = new List<Stats>();
}

[System.Serializable]
public class Stats {
    public Item item;
    [Range(0, 100)] public int rate;
    public DropRate droprate;
}
public struct DropRate { public int min, max; }

