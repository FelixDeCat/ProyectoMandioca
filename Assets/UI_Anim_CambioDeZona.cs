using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Anim_CambioDeZona : MonoBehaviour
{
    public static UI_Anim_CambioDeZona instance;

    [SerializeField] AudioClip shium;
    [SerializeField] AudioClip pahh;

    const string CSHIUM = "Shium";
    const string CPAHH = "Pahh";

    [SerializeField] TextMeshProUGUI maintext;
    [SerializeField] TextMeshProUGUI auxiliartext;
    Animator myAnim;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        AudioManager.instance.GetSoundPool(CSHIUM, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, shium);
        AudioManager.instance.GetSoundPool(CPAHH, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, pahh);
    }
    public static void BeginAnimation(string val) => instance.BeginAnim(val);

    void BeginAnim(string val)
    {
        myAnim.Play("CambioZonaAnim");
        maintext.text = val;
        auxiliartext.text = val;
    }

    public void ANIM_EVENT_SHIIIIUUUUUUMMMMMM()
    {
        AudioManager.instance.PlaySound(CSHIUM);
    }
    public void ANIM_EVENT_PAHHHHHHHHHHHHHHHH()
    {
        AudioManager.instance.PlaySound(CPAHH);
    }

}
