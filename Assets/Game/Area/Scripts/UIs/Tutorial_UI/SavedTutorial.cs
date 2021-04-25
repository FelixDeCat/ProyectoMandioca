using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SavedTutorial : MonoBehaviour
{
    [SerializeField] UI_Anim_Code tutoPrefab = null;

    [SerializeField] GameObject leftArrow = null;
    [SerializeField] GameObject rightArrow = null;
    [SerializeField] GameObject backInfo = null;

    [SerializeField] Button myButton = null;
    [SerializeField] Button settingsButton = null;
    [SerializeField] Button cheatsButton = null;

    List<UI_Anim_Code> myTutorials = new List<UI_Anim_Code>();
    Dictionary<UI_Anim_Code, TMP_SpriteAsset> joystickSpriteAsset = new Dictionary<UI_Anim_Code, TMP_SpriteAsset>();
    Dictionary<UI_Anim_Code, TMP_SpriteAsset> keyboardSpriteAsset = new Dictionary<UI_Anim_Code, TMP_SpriteAsset>();

    bool opened;
    bool isJoystick;
    bool canChange;
    int currentIndex;
    int currentDir;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, ChangeInput);
    }

    public void CanOpen() 
    {
        if (myButton.interactable == true || myTutorials.Count == 0) return;

        myButton.interactable = true;
        var newNav = new Navigation();
        newNav.mode = Navigation.Mode.Explicit;
        newNav.selectOnDown = cheatsButton;
        newNav.selectOnUp = settingsButton;
        myButton.navigation = newNav;

        var newNav2 = new Navigation();
        newNav2.mode = Navigation.Mode.Explicit;
        newNav2.selectOnDown = myButton;
        newNav2.selectOnUp = settingsButton.navigation.selectOnUp;
        settingsButton.navigation = newNav2;

        var newNav3 = new Navigation();
        newNav3.mode = Navigation.Mode.Explicit;
        newNav3.selectOnDown = cheatsButton.navigation.selectOnDown;
        newNav3.selectOnUp = myButton;
        cheatsButton.navigation = newNav3;
    }


    void ChangeInput(params object[] parameters)
    {
        var b = parameters[0] as string;
        isJoystick = b == "Joystick";

        if (opened)
            RefreshSpriteAsset();
    }

    void RefreshSpriteAsset()
    {
        var tmpro = myTutorials[currentIndex].GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < tmpro.Length; i++)
        {
            tmpro[i].spriteAsset = isJoystick ? joystickSpriteAsset[myTutorials[currentIndex]] : keyboardSpriteAsset[myTutorials[currentIndex]];
        }
    }

    public void SaveTutorial(TutorialSettings settings)
    {
        var newTuto = Instantiate<UI_Anim_Code>(tutoPrefab, transform);
        var text = newTuto.GetComponentsInChildren<TextMeshProUGUI>();

        text[0].text = settings.mainDialog;
        text[1].text = settings.title;

        newTuto.GetComponentsInChildren<Image>()[1].sprite = settings.images[0];

        myTutorials.Add(newTuto);
        joystickSpriteAsset.Add(newTuto, settings.joystickSpriteAsset);
        keyboardSpriteAsset.Add(newTuto, settings.keyBoardSpriteAsset);

        newTuto.AddCallbacks(CanChange, () => { });
    }

    public void Close()
    {
        if (!opened) return;
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        backInfo.SetActive(false);
        opened = false;

        myTutorials[currentIndex].Close();

        for (int i = 0; i < currentIndex + 1; i++)
        {
            myTutorials[i].ChangeAppearAndDisappearSide(UI_Anim_Code.AppearSide.Right, UI_Anim_Code.AppearSide.Left);
        }
    }

    public void Open()
    {
        if (opened) return;
        opened = true;
        rightArrow.SetActive(myTutorials.Count > 1);
        backInfo.SetActive(true);
        currentIndex = 0;

        myTutorials[currentIndex].Open();

        RefreshSpriteAsset();
    }

    public void ChangeTutorial(int dir)
    {
        if (!canChange) return;

        if ((dir < 0 && currentIndex == 0) || (dir > 0 && currentIndex == myTutorials.Count - 1)) return;

        if (dir == -1)
            myTutorials[currentIndex].ChangeAppearAndDisappearSide(UI_Anim_Code.AppearSide.Right, UI_Anim_Code.AppearSide.Right);
        else
            myTutorials[currentIndex].ChangeAppearAndDisappearSide(UI_Anim_Code.AppearSide.Left, UI_Anim_Code.AppearSide.Left);

        myTutorials[currentIndex].Close();

        currentIndex += dir;

        myTutorials[currentIndex].Open();
        canChange = false;

        rightArrow.SetActive(currentIndex < myTutorials.Count - 1);
        leftArrow.SetActive(currentIndex > 0);

        RefreshSpriteAsset();
    }

    void CanChange()
    {
        canChange = true;
    }
}
