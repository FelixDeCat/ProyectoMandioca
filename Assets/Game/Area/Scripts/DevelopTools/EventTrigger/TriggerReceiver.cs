using UnityEngine;
public abstract class TriggerReceiver : MonoBehaviour
{
    [SerializeField] protected bool has_one_Shot = false;
    bool oneshot;

    public void Execute()
    {
        if (has_one_Shot)
        {
            if (!oneshot)
            {
                oneshot = true;
                OnExecute();
            }
        }
        else
        {
            OnExecute();
        }
    }

    protected abstract void OnExecute();
}
