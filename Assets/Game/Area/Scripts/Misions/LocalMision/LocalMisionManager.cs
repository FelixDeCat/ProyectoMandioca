using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMisionManager : MonoBehaviour
{
    public static LocalMisionManager instance;
    private void Awake() => instance = this;

    public void AddItemMision(int Id, int Index)
    {

    }
}
