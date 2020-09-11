using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] MainMenuButtons selectorButtons = null;

    public Animator animLoadM;
    public Animator animSttngsM;
    public Animator animCredits;

    public Animator fadeAnim;

    public Button[] mainButtons;

    private Animator _currentAnim;

    [SerializeField] DefaultMenuAnim buttons_joystick_panel = null;
    [SerializeField] DefaultMenuAnim buttons_Keyboard_panel = null;

    [Header("Si esta activo usa el input del mouse")]
    public bool _activeRotation;

    public string sceneGym = "Gym";
    public string sceneGym2 = "Gym";
    public string sceneGym3 = "Gym";
    public string sceneGym4 = "Art muestra";
    public string sceneBlocking = "TerrainTestGonzaSinChar";


    private void Start()
    {
        Main.instance.GetChar().transform.position = new Vector3(1000, 1000, 1000);
    }

    public void StarButton()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        inputs.ChangeRotation(_activeRotation);
        LoadSceneHandler.instance.LoadAScene(sceneBlocking);
        gameObject.SetActive(false);
    }
    public void LoadButton() //aca abro un mini menu con animator ,por el momento podrian acceder a las demas escenas desde aca
    {
        _currentAnim = animLoadM;
        animLoadM.SetTrigger("Open");
        foreach (var item in mainButtons)
        {
            item.interactable = false;
        }
        fadeAnim.SetTrigger("MenuFade");
    }

    public void Settings()
    {
        //que lo codee su vieja
    }
    public void Credits()
    {
        animCredits.SetTrigger("Open");
        _currentAnim = animCredits;
        foreach (var item in mainButtons)
        {
            item.interactable = false;
        }
        fadeAnim.SetTrigger("MenuFade");
    }

    public void ReactivateButtons(Animator currentAnim)//cuando quiera salir de los miniMenues
    {
        fadeAnim.SetTrigger("Off");
        currentAnim.SetTrigger("Off");
        foreach(var item in mainButtons)
        {
            item.interactable = true;
        }
    }
    private void Update()
    {
        if (_currentAnim != null && Input.GetKeyDown(KeyCode.Escape) || _currentAnim != null && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            ReactivateButtons(_currentAnim);
            selectorButtons.SelectButton(mainButtons[0].gameObject);
            _currentAnim = null;
        }
    }
    public void goToGym()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        inputs.ChangeRotation(_activeRotation);
        LoadSceneHandler.instance.LoadAScene(sceneGym);
        gameObject.SetActive(false);
    }
    public void goToGym2()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        inputs.ChangeRotation(_activeRotation);
        LoadSceneHandler.instance.LoadAScene(sceneGym2);
        gameObject.SetActive(false);
    }
    public void goToGym3()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        inputs.ChangeRotation(_activeRotation);
        LoadSceneHandler.instance.LoadAScene(sceneGym3);
        gameObject.SetActive(false);
    }
    public void goToGym4()
    {        
        CharacterInput inputs = Main.instance.GetChar().getInput;
        inputs.ChangeRotation(_activeRotation);
        LoadSceneHandler.instance.LoadAScene(sceneGym4);
        gameObject.SetActive(false);
    }
}
