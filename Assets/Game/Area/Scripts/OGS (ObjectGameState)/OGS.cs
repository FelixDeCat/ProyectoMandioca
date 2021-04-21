using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OGS : MonoBehaviour
{
    string UniqueIdentifier = "null";
    public string ID { get => UniqueIdentifier; }

    [SerializeField] string CustomID = "";

    public void ChangeState(string s)
    {
        OGSManager.Raise(gameObject.scene.name, s);
    }
    public void ChangeState(byte val)
    {

    }
    public void ChangeState(params object[] objs)
    {

    }

}
