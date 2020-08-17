using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LocalMisionManager : MonoBehaviour
{
    public static LocalMisionManager instance;
    private void Awake() => instance = this;

    public List<LocalItemMision> localmisions = new List<LocalItemMision>();
    private void Start()
    {
        localmisions = GetComponentsInChildren<LocalItemMision>().ToList();
        OnMissionsChange();
    }

    public void OnMissionsChange()
    {
        foreach (var l in localmisions)
        {
            l.Refresh();
        }
    }
}
