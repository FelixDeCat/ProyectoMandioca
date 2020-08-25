using UnityEngine;
public class DealMision : MonoBehaviour
{
    public int ID;
    public void OnExecute() => MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(ID));
}
