using UnityEngine;
using UnityEngine.Events;
public class DealMision : MonoBehaviour
{
    public int ID;
    public UnityEvent UE_EndMision;
    public void OnExecute()
    {
        MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(ID), EndMision);
    }

    public void EndMision(Mision m)
    {
        UE_EndMision.Invoke();
    }
}
