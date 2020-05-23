using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class ManagerLoadBase : MonoBehaviour
{

    private void Awake()
    {
        StartLoad();
    }

    public void StartLoad()
    {
        IEnumerable myFunc;
        myFunc = GetFunc();

        StartCoroutine(myFunc.GetEnumerator());
    }

    IEnumerable GetFunc()
    {
        yield return MyFunc();
        yield return MyFunc();
        yield return MyFuncWait();
        yield return MyFunc();
    }

    IEnumerator MyFunc()
    {
        Debug.Log("asd");
        yield return null;
    }

    IEnumerator MyFuncWait()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("waaaaaaitessss");
        yield return null;
    }
}
