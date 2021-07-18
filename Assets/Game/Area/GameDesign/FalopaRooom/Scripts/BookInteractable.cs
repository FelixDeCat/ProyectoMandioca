using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteractable : Interactable
{
    private const string varName = "_FresnelPowa1";

    float originalValue = 5;
    [SerializeField] MeshRenderer[] myrenders = new MeshRenderer[0];
    [SerializeField] Sprite bookImage = null;
    public bool automaticSelectRenders = false;

    public string actionName = "hold to grab";
    public bool isOpen;

    [SerializeField] AudioClip _feedBack = null;
    [SerializeField] string achieveID = "BookRead";
    bool achieveActive;
    private void Start()
    {
        if (automaticSelectRenders) myrenders = GetComponentsInChildren<MeshRenderer>();
        if (_feedBack)
            AudioManager.instance.GetSoundPool(_feedBack.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _feedBack);
        SetPredicate(() => !executing);
    }
    public override void OnEnter(WalkingEntity entity)
    {
        ContextualBarSimple.instance.Show();
        //ContextualBarSimple.instance.Set_Sprite_Photo(image_to_interact);
        ContextualBarSimple.instance.Set_Sprite_Button_Custom(InputImageDatabase.InputImageType.interact);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        if (!isOpen)
        {
            if (Main.instance.gameUiController.OpenCustomImage(bookImage))
            {
                isOpen = true;
                if(!achieveActive)
                {
                    achieveActive = true;
                    AchievesManager.instance.CompleteAchieve(achieveID);
                }
            }
        }
    }

    public override void OnInterrupt()
    {
    }

    public override void OnExit(WalkingEntity collector)
    {
        if (isOpen)
        {
            Main.instance.gameUiController.CloseCustomImage();
            isOpen = false;
        }
        ContextualBarSimple.instance.Hide();
    }

    public void FresnelOn()
    {
        for (int i = 0; i < myrenders.Length; i++) myrenders[i].material.SetFloat(varName, originalValue);
    }
}
