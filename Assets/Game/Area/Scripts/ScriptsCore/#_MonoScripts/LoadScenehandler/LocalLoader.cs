using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class LocalLoader : MonoBehaviour
{
    [SerializeField] List<LoadComponent> loadCOmponents;
    public int MaxCount { get => loadCOmponents.Count; }
    public List<LoadComponent> GetLoaders() => loadCOmponents;
}
