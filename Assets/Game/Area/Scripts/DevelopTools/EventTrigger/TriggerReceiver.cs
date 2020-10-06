using UnityEngine;
public abstract class TriggerReceiver : MonoBehaviour
{
    [SerializeField] protected bool has_one_Shot = false;
    bool oneshot;

    public void Execute(params object[] parameters)
    {
        //Debug.Log("ENTRO AL EXECUTE");

        if (has_one_Shot)
        {
            if (!oneshot)
            {
                oneshot = true;
                OnExecute(parameters);
            }
        }
        else
        {
            OnExecute(parameters);
        }
    }

    protected abstract void OnExecute(params object[] parameters);
}
