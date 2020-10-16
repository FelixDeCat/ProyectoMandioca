using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextualBarSimple : MonoBehaviour
{
    public static ContextualBarSimple instance;
    private void Awake()
    {
        instance = this;
        myRect = GetComponent<RectTransform>();
        Catch_Initial_Values();
    }
    private void Start()
    {
        canvas_group.alpha = 0;
        generic_bar_fill.Configure(1, 0.01f);
        generic_bar_fill.SetValue(0);
    }

    [SerializeField] GenericBar_Sprites generic_bar_fill;
    [Header("Auxiliar Components")]
    [SerializeField] Image photo_image;

    [Header("Animation")]
    public CanvasGroup canvas_group = null;
    public float transition_animation_time = 1f;

    RectTransform myRect;

    InputImageDatabase.InputImageType type;

    void Catch_Initial_Values()
    {
        Reset_All();
    }

    void Reset_All()
    {
        generic_bar_fill.Configure(1, 0.01f);
        generic_bar_fill.SetValue(0);
        photo_image.sprite = null;
    }

    public ContextualBarSimple Show()
    {
        myRect.ForceUpdateRectTransforms();
        canvas_group.alpha = 1;
        return this;
    }
    public ContextualBarSimple Hide()
    {
        canvas_group.alpha = 0;
        Reset_All();
        return this;
    }

    public void ResetBar() { generic_bar_fill.Configure(1, 0.01f); generic_bar_fill.SetValue(0); }
    public ContextualBarSimple Set_Sprite_Photo(Sprite val, bool hasBackground = true) { photo_image.sprite = val; return this; }
    public ContextualBarSimple Set_Values_Load_Bar(float max, float current) { generic_bar_fill.Configure(max, 0.01f); generic_bar_fill.SetValue(current); return this; }

}
