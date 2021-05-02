using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial_UIController : MonoBehaviour
{
    public static Tutorial_UIController instance { get; private set; }

    [SerializeField] UI_Anim_Code mainHud = null;

    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI mainDialogText = null;
    [SerializeField] Image[] uiImages = new Image[0];
    [SerializeField] GameObject continueJoystick = null;
    [SerializeField] GameObject continueKeyboard = null;

    TMP_SpriteAsset spriteAssetKey;
    TMP_SpriteAsset spriteAssetJoystick;
    List<Tutorial_UIController> tutorialActives = new List<Tutorial_UIController>();

    bool isJoystick;
    bool tutoShowing;
    bool canUpdate;

    private void Start()
    {
        instance = this;
        mainHud.AddCallbacks(() => canUpdate = true, EndTutorial);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, ChangeSpriteAsset);
    }

    void ChangeSpriteAsset(params object[] param)
    {
        var b = param[0] as string;

        isJoystick = b == "Joystick";

        if (tutoShowing)
        {
            mainDialogText.spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;
            continueJoystick.SetActive(isJoystick);
            continueKeyboard.SetActive(!isJoystick);
        }
    }

    public void SetNewTutorial(TutorialSettings settings)
    {
        if (settings == null) return;

        tutoShowing = true;
        PauseManager.Instance.tutorialHud.SaveTutorial(settings);
        PauseManager.Instance.Pause();
        titleText.text = settings.title;
        mainDialogText.text = settings.mainDialog;
        spriteAssetJoystick = settings.joystickSpriteAsset;
        spriteAssetKey = settings.keyBoardSpriteAsset;

        continueJoystick.SetActive(isJoystick);
        continueKeyboard.SetActive(!isJoystick);

        mainDialogText.spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;

        for (int i = 0; i < uiImages.Length; i++)
        {
            if (i >= settings.images.Length) break;

            uiImages[i].sprite = settings.images[i];
        }

        mainHud.Open();
    }

    void EndTutorial()
    {
        tutoShowing = false;
        PauseManager.Instance.Resume();
    }

    private void Update()
    {
        if (canUpdate && Input.GetButtonDown("Submit"))
        {
            mainHud.Close();
            canUpdate = false;
        }
    }
}
