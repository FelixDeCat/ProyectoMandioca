using UnityEngine;
using UnityEditor;
[System.Serializable]
public abstract class Switcheable : MonoBehaviour
{
    public void OnFade(float val) { ABSOnFade(val); }
    public void OnTurnOn() { ABSOnTurnON(); }
    public void OnTurnOff() { ABSOnTurnOff(); }

    public abstract void ABSOnFade(float f);
    public abstract void ABSOnTurnON();
    public abstract void ABSOnTurnOff();

    //public override string ToString()
    //{
    //    return "Switcheable: " + gameObject.name;
    //}

    public override string ToString()
    {
        return base.ToString();
    }

}
