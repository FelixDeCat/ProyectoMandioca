using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenCounter : UI_Base
{
    public TextMeshProUGUI info;
    public TextMeshProUGUI cant;
    public Animator animator;

    public static GenCounter instance;
    private void Awake() => instance = this;

    public static void OpenCounter() { instance.Open(); }
    public static void CloseCounter() { instance.Close(); }
    public static void SetCounterInfo(string info_to_count, int current_val, int max_val, bool anim = false) => instance.SetInfo(info_to_count, current_val, max_val, anim);
    void SetInfo(string info_to_count, int current_val, int max_val, bool anim = false)
    {
        info.text = info_to_count;
        cant.text = current_val.ToString() + " / " + max_val.ToString();
        if (anim)
        {
            animator.Play("CountAnim");
        }
    }

    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
}
