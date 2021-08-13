using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Tools;
using DevelopTools.UI;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    int pauseCount = 0;
    public static PauseManager Instance { get; private set; }

    List<IPauseable> pausingPlayObjects = new List<IPauseable>();
    [SerializeField] UI_Anim_Code pauseHud = null;
    [SerializeField] GameObject backgroundPause = null;
    [SerializeField] GameObject mainFirstButton = null;
    [SerializeField] GameObject mainButtons = null;
    [SerializeField] GameObject cheatsHud = null;
    [SerializeField] Settings settingsHud = null;
    [SerializeField] ScrollViewSetter achievesHud = null;
    public SavedTutorial tutorialHud = null;

    public bool inPauseHud;
    bool inSettings;
    bool inTutorial;
    bool canPress;
    bool pause;

    private void Awake()
    {
        Instance = this;

        pauseHud.AddCallbacks(() => { }, () => { mainButtons.SetActive(false); Resume(); });
    }

    private void Start()
    {
        cheatsHud.SetActive(false);
    }

    public void PauseHud()
    {
        Pause();
        backgroundPause.SetActive(true);
        pauseHud.Open();
        mainButtons.SetActive(true);
        MyEventSystem.instance.SelectGameObject(mainFirstButton);
        canPress = false;
        inPauseHud = true;
        tutorialHud.CanOpen();
        AudioAmbienceSwitcher.instance.Pause();
    }

    public void Pause()
    {
        if (pause)
        {
            pauseCount += 1;
            return;
        }
        for (int i = 0; i < pausingPlayObjects.Count; i++)
        {
            pausingPlayObjects[i].Pause();
        }

        ParticlesManager.Instance.PauseParticles();
        AudioManager.instance.PauseSounds();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Main.instance.GetChar().getInput.inMenu = true;
        pause = true;
        pauseCount = 1;
    }

    public void ResumeHud()
    {
        backgroundPause.gameObject.SetActive(false);
        pauseHud.Close();
        MyEventSystem.instance.SelectGameObject(null);
        inPauseHud = false;
        AudioAmbienceSwitcher.instance.Resume();
    }

    public void Resume()
    {
        if (!pause)
            return;
        else
        {
            pauseCount -= 1;
            if (pauseCount > 0) return;
        }

        for (int i = 0; i < pausingPlayObjects.Count; i++)
            pausingPlayObjects[i].Resume();

        ParticlesManager.Instance.ResumeParticles();
        AudioManager.instance.ResumeSounds();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Main.instance.GetChar().getInput.inMenu = false;
        pause = false;
    }

    public void SettingsScreen()
    {
        mainButtons.SetActive(false);
        settingsHud.gameObject.SetActive(true);
        settingsHud.OpenGameplay();
        inSettings = true;
    }

    public void TutorialScreen()
    {
        mainButtons.SetActive(false);
        tutorialHud.Open();
        inTutorial = true;
        pauseHud.gameObject.SetActive(false);
    }

    public void AchievesScreen()
    {
        mainButtons.SetActive(false);
        achievesHud.gameObject.SetActive(true);
        achievesHud.Open();
    }

    public void Cheats()
    {
        mainButtons.SetActive(false);
        cheatsHud.SetActive(true);
        Debug_UI_Tools.instance.Toggle(true);
    }

    public void BackToPause()
    {
        pauseHud.gameObject.SetActive(true);
        mainButtons.SetActive(true);
        Debug_UI_Tools.instance.Toggle(false);
        cheatsHud.SetActive(false);
        settingsHud.gameObject.SetActive(false);
        if (achievesHud.gameObject.activeSelf)
        {
            achievesHud.Close();
            achievesHud.gameObject.SetActive(false);
        }
        tutorialHud.Close();
        inTutorial = false;
        inSettings = false;
        MyEventSystem.instance.SelectGameObject(mainFirstButton);
    }

    public void ReturnToMenu()
    {
        var myGameCores = FindObjectsOfType<DontDestroy>().Where(x => x.transform != transform.parent).ToArray();
        NewSceneStreamer.instance?.RemoveToSceneLoaded();

        for (int i = 0; i < myGameCores.Length; i++)
            Destroy(myGameCores[i].gameObject);
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(transform.parent.gameObject);
    }

    private void Update()
    {
        if (!inPauseHud) return;

        if (Input.GetButtonDown("Back") && canPress)
        {
            if (mainButtons.activeSelf)
                ResumeHud();
            else
                BackToPause();
        }

        if (inSettings)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4)) settingsHud.ChangeScreen(-1);
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) settingsHud.ChangeScreen(1);
        }

        if (inTutorial)
        {
            var dir = Input.GetAxis("Horizontal");

            if (dir > 0.5f || dir < -0.5f)
                tutorialHud.ChangeTutorial(dir > 0 ? 1 : -1);
        }

        if (Input.GetButtonDown("Pause") && canPress)
        {
            BackToPause();
            ResumeHud();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.H))
            Cheats();

        if (!canPress) canPress = true;
    }

    public void AddToPause(IPauseable po) => pausingPlayObjects.Add(po);
    public void RemoveToPause(IPauseable po) => pausingPlayObjects.Remove(po);
}
