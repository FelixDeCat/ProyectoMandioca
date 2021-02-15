using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextualBar : MonoBehaviour
{
    public static ContextualBar instance;
    private void Awake()
    {
        instance = this;
        myRect = GetComponent<RectTransform>();
        Catch_Initial_Values();
    }
    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, OnChange);

    }

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI action_text = null;
    [SerializeField] TextMeshProUGUI name_interaction_text = null;
    [SerializeField] TextMeshProUGUI extra_description_text = null;
    [SerializeField] GenericBar_Sprites generic_bar_fill = null;
    [Header("Auxiliar Components")]
    [SerializeField] Image photo_image = null;
    [SerializeField] Image button_image = null;
    [Header("Customization")]
    [SerializeField] Image bkg_photo = null;
    [SerializeField] Image bkg_load_bar = null;
    [SerializeField] Image load_bar = null;

    [Header("Animation")]
    public CanvasGroup canvas_group = null;
    public float transition_animation_time = 1f;
    float timer_transition = 0;
    bool anim_transition = false;
    bool anim_on_way = false;
    bool has_animation = false;

    Sprite defaultButton;
    Color default_bkg_photo_color;
    Color default_bkg_load_bar_color;
    Color default_load_bar_color;

    RectTransform myRect;

    bool modif_type = false;
    InputImageDatabase.InputImageType type;

    void Catch_Initial_Values()
    {
        defaultButton = button_image.sprite;
        default_bkg_photo_color = bkg_photo.color;
        default_bkg_load_bar_color = bkg_load_bar.color;
        default_load_bar_color = load_bar.color;
        Reset_All();
    }

    void Reset_All()
    {
        action_text.text = "";
        name_interaction_text.text = "";
        extra_description_text.text = "";
        generic_bar_fill.Configure(1, 0.01f);
        generic_bar_fill.SetValue(1);
        photo_image.sprite = null;
        button_image.sprite = defaultButton;
        bkg_photo.color = default_bkg_photo_color;
        bkg_photo.gameObject.SetActive(false);
        bkg_load_bar.color = default_bkg_load_bar_color;
        load_bar.color = default_load_bar_color;
        has_animation = true;
    }

    public ContextualBar Show()
    {
        myRect.ForceUpdateRectTransforms();
        if (has_animation) { anim_transition = true; anim_on_way = true; timer_transition = 0; }
        else canvas_group.alpha = 1;
        return this;
    }
    public ContextualBar Hide()
    {
        modif_type = false;
        if (has_animation) { anim_transition = true; anim_on_way = false; timer_transition = 1; }
        else canvas_group.alpha = 0;
        return this;
    }

    public ContextualBar Set_Text_Action(string val) { action_text.text = val; return this; }
    public ContextualBar Set_Text_Interaction_Name(string val) { name_interaction_text.text = val; return this; }
    public ContextualBar Set_Text_ExtraInfo(string val) { extra_description_text.text = val; return this; }
    public ContextualBar Set_Sprite_Photo(Sprite val, bool hasBackground = true) { photo_image.sprite = val; if (hasBackground) bkg_photo.gameObject.SetActive(hasBackground); return this; }
    public ContextualBar Set_Sprite_Button_Default() { button_image.sprite = defaultButton; return this; }
    public ContextualBar Set_Sprite_Button_Custom(Sprite val) { button_image.sprite = val; return this; }
    public ContextualBar Set_Sprite_Button_Custom(InputImageDatabase.InputImageType val) { type = val; modif_type = true; button_image.sprite = InputImageDatabase.instance.GetSprite(type); return this; }
    public ContextualBar Set_Color_Field_Photo_Background(Color val) { bkg_photo.color = val; return this; }
    public ContextualBar Set_Color_Field_Load_Bar_Background(Color val) { bkg_load_bar.color = val; return this; }
    public ContextualBar Set_Color_Field_Load_Bar(Color val) { load_bar.color = val; return this; }
    public ContextualBar Set_Values_Load_Bar(float max, float current) { generic_bar_fill.Configure(max, 0.01f); generic_bar_fill.SetValue(current); return this; }
    public ContextualBar Set_Has_Animation(bool val = true) { has_animation = val; return this; }


    // PRIVATES
    private void OnChange(params object[] p) { if (!modif_type) return; button_image.sprite = InputImageDatabase.instance.GetSprite(type); }

    private void Update()
    {
        if (anim_transition)
        {
            if (anim_on_way)
            {
                if (timer_transition < transition_animation_time)
                {
                    timer_transition = timer_transition + 1 * Time.deltaTime;
                    canvas_group.alpha = timer_transition / transition_animation_time;
                }
                else { timer_transition = 0; anim_transition = false; }
            }
            else
            {
                if (timer_transition > 0)
                {
                    timer_transition = timer_transition - 1 * Time.deltaTime;
                    canvas_group.alpha = timer_transition / transition_animation_time;
                }
                else { timer_transition = 1; anim_transition = false; }
            }
        }
    }
}
