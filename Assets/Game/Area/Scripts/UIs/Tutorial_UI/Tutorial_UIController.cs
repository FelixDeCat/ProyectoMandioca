using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial_UIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI mainDialogText = null;
    [SerializeField] Image[] uiImages = new Image[0];

    TMP_SpriteAsset spriteAssetKey;
    TMP_SpriteAsset spriteAssetJoystick;

    bool isJoystick;
    bool tutoShowing;

    void ChangeSpriteAsset(params object[] param)
    {
        var b = param[0] as string;

        isJoystick = b == "Joystick";

        if (tutoShowing)
        {
            mainDialogText.spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;
        }
    }

    public void SetNewTutorial(TutorialSettings settings)
    {
        tutoShowing = true;

        titleText.text = settings.title;
        mainDialogText.text = settings.mainDialog;
        spriteAssetJoystick = settings.joystickSpriteAsset;
        spriteAssetKey = settings.keyBoardSpriteAsset;

        mainDialogText.spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;

        for (int i = 0; i < uiImages.Length; i++)
        {
            if (i >= settings.images.Length) break;

            uiImages[i].sprite = settings.images[i];
        }
    }

    void EndTutorial()
    {
        tutoShowing = false;
    }
}
