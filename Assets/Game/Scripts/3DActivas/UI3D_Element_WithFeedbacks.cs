using UnityEngine;
public class UI3D_Element_WithFeedbacks : UI3D_Element
{
    //esto esta medio confuso... arreglar nombres
    [Header("UI3D_Element_WithFeedbacks")]
    public FeedbackOneInteractBase[] feedbackOnSelect;
    public FeedbackInteractBase[] feedbacks;
    public FeedbackInteractBase[] feedback_is_usable;

    private void Start()
    {
        foreach (var f in feedbacks)
        {
            f.Hide();
        }
    }

    public void SetIsUsable() { foreach (var f in feedback_is_usable) f.Show(); }
    public void SetIsNotUsable() { foreach (var f in feedback_is_usable) f.Hide(); }

    protected override void Custom_UI_Event_OnSelect() { base.Custom_UI_Event_OnSelect(); foreach (var f in feedbackOnSelect) f.Execute(); foreach (var f in feedbacks) f.Show(); }
    protected override void Custom_UI_Event_OnDeselect() { base.Custom_UI_Event_OnDeselect(); foreach (var f in feedbacks) f.Hide(); }
    protected override void Custom_UI_Event_OnSubmit() { base.Custom_UI_Event_OnSubmit();  }
    protected override void Custom_UI_Event_OnUpdateSelect() { base.Custom_UI_Event_OnUpdateSelect(); }
    protected override void Custom_UI_Click_OnClick() { base.Custom_UI_Click_OnClick(); }
    protected override void Custom_UI_Click_OnHoverEnter() { base.Custom_UI_Click_OnHoverEnter(); foreach (var f in feedbacks) f.Show(); }
    protected override void Custom_UI_Click_OnHoverExit() { base.Custom_UI_Click_OnHoverExit(); foreach(var f in feedbacks) f.Hide(); }
    protected override void Custom_UI_Click_OnClickDown() { base.Custom_UI_Click_OnClickDown(); }
    protected override void Custom_UI_Click_OnClickUp() { base.Custom_UI_Click_OnClickUp(); }

}
