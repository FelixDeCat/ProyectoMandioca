using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class LocalLoader : MonoBehaviour
{
    [SerializeField] List<LoadComponent> loadCOmponents =  new List<LoadComponent>();
    public int MaxCount { get => loadCOmponents.Count; }
    public List<LoadComponent> GetLoaders() => loadCOmponents;
}
