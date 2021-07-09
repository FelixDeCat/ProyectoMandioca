using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] Settings settingsScreen = null;
    [SerializeField] Button backButton = null;
    [SerializeField] ScrollViewSetter achieves = null;

    public Animator animLoadM;
    public Animator animSttngsM;
    public Animator animCredits;

    public Animator fadeAnim;

    public Button[] mainButtons;
    [SerializeField] Button loadScreenButton = null;

    private Animator _currentAnim;

    public string sceneGym = "Gym";
    public string sceneGym2 = "Gym";
    public string sceneGym3 = "Gym";
    public string sceneGym4 = "Art muestra";
    public string sceneGym5 = "Arena";
    public string sceneGym6 = "MainScene";
    public string sceneBlocking = "TerrainTestGonzaSinChar";

    bool inSettings = true;


    private void Start()
    {
        MyEventSystem.instance.SelectGameObject(mainButtons[0].gameObject);
        Main.instance.GetChar().transform.position = new Vector3(1000, 1000, 1000);
        LoadSceneHandler.instance.Off_LoadScreen();
        Invoke("FadeOff",1f);
    }

    void FadeOff()
    {
        Fades_Screens.instance.FadeOff(() => { });
    }

    public void StarButton()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneBlocking,false);
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
        backButton.gameObject.SetActive(true);
        MyEventSystem.instance.SelectGameObject(loadScreenButton.gameObject);
    }

    public void Settings()
    {
        settingsScreen.gameObject.SetActive(true);
        _currentAnim = animSttngsM;
        _currentAnim.SetTrigger("Open");
        foreach (var item in mainButtons) item.interactable = false;
        fadeAnim.SetTrigger("MenuFade");
        settingsScreen.OpenGameplay();

        inSettings = true;

        backButton.gameObject.SetActive(true);
    }

    public void OpenAchieves()
    {
        foreach (var item in mainButtons) item.interactable = false;
        achieves.gameObject.SetActive(true);
        achieves.Open();
    }

    public void CloseAchieves()
    {
        foreach (var item in mainButtons) item.interactable = true;
        achieves.Close();
        achieves.gameObject.SetActive(false);
        MyEventSystem.instance.SelectGameObject(mainButtons[0].gameObject);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
    public void Credits()
    {
        animCredits.SetTrigger("Open");
        _currentAnim = animCredits;
        foreach (var item in mainButtons) item.interactable = false;
        fadeAnim.SetTrigger("MenuFade");
    }

    public void ReactivateButtons(Animator currentAnim)//cuando quiera salir de los miniMenues
    {
        fadeAnim.SetTrigger("Off");
        currentAnim?.SetTrigger("Off");
        foreach(var item in mainButtons) item.interactable = true;
    }

    private void Update()
    {
        //if (_currentAnim != null && Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    StartCoroutine(BackCoroutine(0.2f));
        //}
        if(_currentAnim != null && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Back();
        }

        if (inSettings)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4)) settingsScreen.ChangeScreen(-1);
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) settingsScreen.ChangeScreen(1);
        }
    }

   public void Back()
   {
        ReactivateButtons(_currentAnim);
        MyEventSystem.instance.SelectGameObject(mainButtons[0].gameObject);
        _currentAnim = null;
        backButton.gameObject.SetActive(false);
    }

    public void goToGym()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneGym,false);
        gameObject.SetActive(false);
    }

    public void goToGym2()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneGym2,false);
        gameObject.SetActive(false);
    }

    public void goToGym3()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneGym3,false);
        gameObject.SetActive(false);
    }

    public void goToGym4()
    {        
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneGym4,false);
        gameObject.SetActive(false);
    }

    public void goToGym5()
    {
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(sceneGym5,false);
        gameObject.SetActive(false);
    }

    public void LoadScene(string s)
    {
        LoadSceneHandler.instance.On_LoadScreen();
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(s,false);
        gameObject.SetActive(false);
    }

    public void ExitToGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
