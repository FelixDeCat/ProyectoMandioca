using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_CurrentItem : UI_Base
{
    public SpotType spot_to_represent;
    [SerializeField] TextMeshProUGUI txt_cant = null;
    [SerializeField] Image img = null;
    
    [SerializeField] GenericBar_Sprites cooldownBar = null;
    [SerializeField] GenericBar_Sprites castingbar = null;
    [SerializeField] ParticleSystem part_endLoad = null;
    [SerializeField] GameObject onPress = null;
    [SerializeField] GameObject HoldThePower = null;

    [SerializeField] Image img_aux = null;
    public float anim_scale_on_use = 0.2f;
    Vector3 originalScale;
    Vector3 scaledScale;


    public void SetItem(string _cant, Sprite _img, bool showNumber = true)
    {
        txt_cant.text = _cant;
        txt_cant.gameObject.SetActive(showNumber);
        img.sprite = _img;
        img_aux.sprite = _img;
        // img_aux.enabled = false;
    }
    
    public void SetCastingBar(float current, float max)
    {
        castingbar?.Configure(max, 0.01f);
        castingbar?.SetValue(current);
    }
    protected override void OnStart()
    {
        originalScale = img_aux.gameObject.transform.localScale;
        var v = img_aux.gameObject.transform.localScale;
        scaledScale = new Vector3(v.x + anim_scale_on_use, v.y + anim_scale_on_use, v.z + anim_scale_on_use);
        img_aux.gameObject.transform.localScale = originalScale;
        img_aux.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        UpdateCoreAnimations();
        UpdateCastingAnimations();
    }

    #region Animation Core Things
    float timer_use;
    bool anim_use;

    
    public void Core_OnUse()
    {
        Debug.Log("ONUSE");
        img_aux.enabled = true;
        img_aux.gameObject.transform.localScale = scaledScale;
        timer_use = 0;
        anim_use = true;
    }
    void UpdateCoreAnimations()
    {
        if (anim_use)
        {
            if (timer_use < 1f)
            {
                timer_use = timer_use + 2 * Time.deltaTime;
                img_aux.gameObject.transform.localScale = Vector3.Lerp(scaledScale, originalScale, timer_use);
                img_aux.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1,1,1,0), timer_use);
            }
            else
            {
                img_aux.gameObject.transform.localScale = originalScale;
                img_aux.color = new Color(1, 1, 1, 0);
                timer_use = 0;
                anim_use = false;
                img_aux.enabled = false;
            }
        }
    }
    #endregion


    #region Animation Cooldown Things
    public void Cooldown_RefreshCurrentValue(float current, float max)
    {
        cooldownBar?.Configure(max, 0.01f);
        cooldownBar?.SetValue(current);
    }
    public void Cooldown_Begin() 
    {
        img.color = new Color(0, 0, 0, 0.5f);
        cooldownBar.SetImageOriginalColor();
    }
    public void Cooldown_End() 
    {
        img.color = Color.white;
        part_endLoad.Play();
        cooldownBar.SetImageColor(new Color(0,0,0,0));
    }
    #endregion

    #region Animation Casting Things
    bool animFail = false;
    float timer_anim_fail = 0;
    public void Casting_RefreshCurrentValue(float current, float max)
    {
        castingbar?.Configure(max, 0.01f);
        castingbar?.SetValue(current);
    }
    public void Casting_Begin()
    {
        StopAnimFail();
        onPress.gameObject.SetActive(true);
    }
    public void Casting_End()
    {
        onPress.gameObject.SetActive(false);
    }
    public void Casting_HoldThePower(bool val)
    {
        HoldThePower?.SetActive(val);
    }
    public void Casting_Fail()
    {
        StopAnimFail();
        animFail = true;
    }
    public void Casting_Fail(int charges = 0)
    {
        StopAnimFail();
        animFail = true;
    }
    void StopAnimFail()
    {
        castingbar.SetImageOriginalColor();
        animFail = false;
        timer_anim_fail = 0;
    }

    void UpdateCastingAnimations()
    {
        if (animFail)
        {
            if (timer_anim_fail < 1f)
            {
                timer_anim_fail = timer_anim_fail + 2f * Time.deltaTime;
                castingbar.SetImageColor(Color.Lerp(Color.red, new Color(1,0,0,0), timer_anim_fail));
            }
            else
            {
                castingbar?.Configure(1, 0.01f);
                castingbar?.SetValue(0);
                StopAnimFail();
            }
        }
    }

    #endregion


    #region unused
    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    
    protected override void OnUpdate() { }
    #endregion
}
