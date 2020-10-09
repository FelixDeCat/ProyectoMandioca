using UnityEngine;
public abstract class TriggerReceiver : MonoBehaviour
{
    [SerializeField] protected bool has_one_Shot = false;
    bool oneshot;

    public void Execute(Collider coll)
    {
        //Debug.Log("ENTRO AL EXECUTE");

        if (has_one_Shot)
        {
            if (!oneshot)
            {
                oneshot = true;
                OnExecute(coll);
            }
        }
        else
        {
            OnExecute(coll);
        }
    }

    protected abstract void OnExecute(Collider coll);
}
