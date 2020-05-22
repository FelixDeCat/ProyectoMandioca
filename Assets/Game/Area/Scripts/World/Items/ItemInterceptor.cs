using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInterceptor : MonoBehaviour
{
    protected Item myitemworld;
    private void Start() => myitemworld = GetComponent<ItemWorld>().item;
    public bool Collect() => OnCollect();
    protected abstract bool OnCollect();
}
