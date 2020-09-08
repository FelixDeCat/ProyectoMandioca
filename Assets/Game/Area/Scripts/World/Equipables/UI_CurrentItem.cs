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


    public void SetItem(string _cant, Sprite _img, bool showNumber = true)
    {
        txt_cant.text = _cant;
        txt_cant.gameObject.SetActive(showNumber);
        img.sprite = _img;
    }

    
    public void SetCastingBar(float current, float max)
    {
        castingbar?.Configure(max, 0.01f);
        castingbar?.SetValue(current);
    }


    #region Animation Cooldown Things
    public void Cooldown_RefreshCurrentValue(float current, float max)
    {
        cooldownBar?.Configure(max, 0.01f);
        cooldownBar?.SetValue(current);
    }

    public void Cooldown_Begin() 
    {
        img.color = new Color(0, 0, 0, 0.5f);
    }
    public void Cooldown_End() 
    {
        img.color = Color.white;
        part_endLoad.Play();
    }
   

    #endregion




    #region unused
    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
    #endregion
}
