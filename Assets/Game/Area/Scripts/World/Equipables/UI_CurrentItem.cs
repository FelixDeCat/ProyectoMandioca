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
    
    [SerializeField] GenericBar_Sprites cooldownBar;
    [SerializeField] GenericBar_Sprites castingbar;
    [SerializeField] ParticleSystem part_endLoad;
    [SerializeField] GameObject onPress;
    [SerializeField] GameObject HoldThePower;

    [SerializeField] Image img_aux = null;
    [SerializeField] Animator anim_OnUse;


    public void SetItem(string _cant, Sprite _img, bool showNumber = true)
    {
        txt_cant.text = _cant;
        txt_cant.gameObject.SetActive(showNumber);
        img.sprite = _img;
        img_aux.sprite = _img;
        img_aux.enabled = false;
    }
    
    public void SetCastingBar(float current, float max)
    {
        castingbar?.Configure(max, 0.01f);
        castingbar?.SetValue(current);
    }
    protected override void OnStart()
    {
        
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
        Debug.Log("Animando");
        anim_OnUse.Play("ImageScale");
    }
    void UpdateCoreAnimations()
    {
        //if (anim_use)
        //{
        //    if (timer_use < 1f)
        //    {
        //        Debug.Log("ONUSE: " + timer_use);
        //        timer_use = timer_use + 1 * Time.deltaTime;
        //        img_aux.gameObject.transform.localScale = Vector3.Lerp(scaledScale, originalScale, timer_use);
        //        img_aux.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1,1,1,0), timer_use);
        //    }
        //    else
        //    {
        //        img_aux.gameObject.transform.localScale = originalScale;
        //        img_aux.color = new Color(1, 1, 1, 0);
        //        timer_use = 0;
        //        anim_use = false;
        //    }
        //}
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
