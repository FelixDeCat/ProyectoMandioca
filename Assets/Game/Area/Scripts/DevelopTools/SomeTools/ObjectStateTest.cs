using UnityEngine;
public class ObjectStateTest : MonoBehaviour
{
    FeedbackInteractBase[] feedbacks;
    private void Awake() { feedbacks = GetComponentsInChildren<FeedbackInteractBase>(); }
    public void ON() { foreach (var f in feedbacks) f.Show(); }
    public void OFF() { foreach (var f in feedbacks) f.Hide(); }
}
