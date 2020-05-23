using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadComponent : MonoBehaviour
{
    public IEnumerator Load()
    {
        yield return OnLoad();
    }

    protected abstract IEnumerator OnLoad();
}
