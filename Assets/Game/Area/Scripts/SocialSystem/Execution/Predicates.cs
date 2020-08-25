using UnityEngine;
using Tools.EventClasses;
public abstract class Predicates : MonoBehaviour
{
    public EventCounterPredicate pred;
    private void Start() => pred.Invoke(CanExecute);
    public abstract bool CanExecute();
}