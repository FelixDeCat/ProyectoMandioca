using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial_UIController : MonoBehaviour
{
    public static Tutorial_UIController instance { get; private set; }

    [SerializeField] UI_Anim_Code mainHud = null;

    [SerializeField] Animator message = null;

    [SerializeField] Transform messageStackParent = null;
    [SerializeField] Animator messagePrefab = null;
    [SerializeField] AudioClip tutoCompleteFeedback = null;

    [SerializeField] float waitToDestroy = 2;

    TutorialSettings current;
    List<TutorialSettings> tutorialStack = new List<TutorialSettings>();
    List<Animator> textStack = new List<Animator>();

    TMP_SpriteAsset spriteAssetKey;
    TMP_SpriteAsset spriteAssetJoystick;

    bool isJoystick;

    private void Start()
    {
        instance = this;
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, ChangeSpriteAsset);
        AudioManager.instance.GetSoundPool(tutoCompleteFeedback.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, tutoCompleteFeedback);
    }

    void ChangeSpriteAsset(params object[] param)
    {
        var b = param[0] as string;

        isJoystick = b == "Joystick";

        if (current != null)
        {
            var array = message.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < array.Length; i++)
            {
                array[i].spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;
            }
        }

        for (int i = 0; i < textStack.Count; i++)
        {
            var array = textStack[i].GetComponentsInChildren<TextMeshProUGUI>();
            for (int x = 0; x < array.Length; x++)
            {
                array[x].spriteAsset = isJoystick ? tutorialStack[i].joystickSpriteAsset : tutorialStack[i].keyBoardSpriteAsset;
            }
        }
    }

    public void SetNewTutorial(TutorialSettings settings)
    {
        if (settings == null) return;

        PauseManager.Instance.tutorialHud.SaveTutorial(settings);
        var array = message.GetComponentsInChildren<TextMeshProUGUI>();
        spriteAssetJoystick = settings.joystickSpriteAsset;
        spriteAssetKey = settings.keyBoardSpriteAsset;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].text = settings.fastMessage;
            array[i].spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;
        }

        if (current != null)
        {
            tutorialStack.Add(current);
            var newText = Instantiate(messagePrefab, messageStackParent);
            var arrayTwo = newText.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 1; i < arrayTwo.Length; i++)
            {
                arrayTwo[i].spriteAsset = isJoystick ? current.joystickSpriteAsset : current.keyBoardSpriteAsset;
                arrayTwo[i].text = current.fastMessage;
            }
            textStack.Add(newText);
        }

        current = settings;

        BlindTutorial(settings, settings.actionToComplete);

        mainHud.Open();
        message.Play("New State");
    }

    public void CompleteTutorial(TutorialSettings tuto)
    {
        if(tuto == current)
        {
            message.Play("CompleteAnim");
            AudioManager.instance.PlaySound(tutoCompleteFeedback.name);

            current = null;
            StartCoroutine(DestroyATutorial(message));
        }
        else if (tutorialStack.Contains(tuto))
        {
            int index = tutorialStack.IndexOf(tuto);

            Animator temp = textStack[index];
            temp.Play("CompleteAnim");

            textStack.RemoveAt(index);
            tutorialStack.RemoveAt(index);
            AudioManager.instance.PlaySound(tutoCompleteFeedback.name);

            StartCoroutine(DestroyATutorial(temp));
        }
    }


    IEnumerator DestroyATutorial(Animator textToDestroy)
    {
        yield return new WaitForSeconds(waitToDestroy);
        if(textToDestroy == message)
        {
            if(tutorialStack.Count > 0)
            {
                mainHud.Open();
                message.Play("New State");
                var array = textToDestroy.GetComponentsInChildren<TextMeshProUGUI>();
                spriteAssetJoystick = tutorialStack[0].joystickSpriteAsset;
                spriteAssetKey = tutorialStack[0].keyBoardSpriteAsset;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].text = tutorialStack[0].fastMessage;
                    array[i].spriteAsset = isJoystick ? spriteAssetJoystick : spriteAssetKey;
                }
                current = tutorialStack[0];
                tutorialStack.Remove(tutorialStack[0]);
                Destroy(textStack[0].gameObject);
                textStack.RemoveAt(0);
            }
            else
            {
                mainHud.Close();
            }
        }
        else
        {
            Destroy(textToDestroy.gameObject);
        }
    }

    void BlindTutorial(TutorialSettings tutorial, ActionRepresentate action)
    {
                Action blindAction = null;

        switch (action)
        {
            case ActionRepresentate.Move:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharMove().Moving -= blindAction; };
                Main.instance.GetChar().GetCharMove().Moving += blindAction;
                break;

            case ActionRepresentate.MoveCamera:
                UnityAction<float> cameraAction = null;
                cameraAction += (x) => { if (x != 0) { CompleteTutorial(tutorial); Main.instance.GetChar().getInput.RightHorizontal.RemoveListener(cameraAction); Main.instance.GetChar().getInput.RightVertical.RemoveListener(cameraAction); } };
                Main.instance.GetChar().getInput.RightHorizontal.AddListener(cameraAction);
                Main.instance.GetChar().getInput.RightVertical.AddListener(cameraAction);
                break;

            case ActionRepresentate.Roll:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharMove().Dash -= blindAction; };
                Main.instance.GetChar().GetCharMove().Dash += blindAction;
                break;

            case ActionRepresentate.Block:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharBlock().callback_OnBlock -= blindAction; };
                Main.instance.GetChar().GetCharBlock().callback_OnBlock += blindAction;
                break;

            case ActionRepresentate.Parry:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharBlock().callback_OnParry -= blindAction; };
                Main.instance.GetChar().GetCharBlock().callback_OnParry += blindAction;
                break;

            case ActionRepresentate.Attack:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharacterAttack().Remove_callback_Normal_attack(blindAction); };
                Main.instance.GetChar().GetCharacterAttack().Add_callback_Normal_attack(blindAction);
                break;

            case ActionRepresentate.Heavy:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharacterAttack().Remove_callback_Heavy_attack(blindAction); };
                Main.instance.GetChar().GetCharacterAttack().Add_callback_Heavy_attack(blindAction);
                break;

            case ActionRepresentate.BashDash:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().GetCharacterAttack().OnBashDash-=blindAction; };
                Main.instance.GetChar().GetCharacterAttack().OnBashDash += blindAction;
                break;

            case ActionRepresentate.UseItem:
                blindAction = () => { CompleteTutorial(tutorial); EquipedManager.instance.OnUseItem -= blindAction; };
                EquipedManager.instance.OnUseItem += blindAction;
                break;

            case ActionRepresentate.Inventory:
                blindAction = () => { CompleteTutorial(tutorial); FastInventory.instance.OnOpenInventory -= blindAction; };
                FastInventory.instance.OnOpenInventory += blindAction;
                break;

            case ActionRepresentate.ActiveOne:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().swordAbilityOnRelease -= blindAction; };
                Main.instance.GetChar().swordAbilityOnRelease += blindAction;
                break;

            case ActionRepresentate.ActiveTwo:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().shieldAbilityOnRelease -= blindAction; };
                Main.instance.GetChar().shieldAbilityOnRelease += blindAction;
                break;

            case ActionRepresentate.Combo:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().combo_system.RemoveCallback_OnExecuteCombo(blindAction); };
                Main.instance.GetChar().combo_system.AddCallback_OnExecuteCombo(blindAction);
                break;

            case ActionRepresentate.SaveWeapons:
                blindAction = () => { CompleteTutorial(tutorial); Main.instance.GetChar().onWeaponTogle -= blindAction; };
                Main.instance.GetChar().onWeaponTogle += blindAction;
                break;
            default:
                break;
        }
    }
}
