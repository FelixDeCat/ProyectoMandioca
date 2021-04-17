using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Tools;
using DevelopTools.UI;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    List<IPauseable> pausingPlayObjects = new List<IPauseable>();
    [SerializeField] UI_Anim_Code pauseHud = null;
    [SerializeField] GameObject backgroundPause = null;
    [SerializeField] GameObject mainFirstButton = null;
    [SerializeField] GameObject mainButtons = null;
    [SerializeField] GameObject cheatsHud = null;
    [SerializeField] Settings settingsHud = null;

    SettingsData data;
    bool inPauseHud;
    bool inSettings;
    bool canPress;

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
    }

    public void Pause()
    {
        for (int i = 0; i < pausingPlayObjects.Count; i++)
        {
            pausingPlayObjects[i].Pause();
        }

        ParticlesManager.Instance.PauseParticles();
        AudioManager.instance.PauseSounds();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Main.instance.GetChar().getInput.inMenu = true;
    }

    public void ResumeHud()
    {
        backgroundPause.gameObject.SetActive(false);
        pauseHud.Close();
        MyEventSystem.instance.SelectGameObject(null);
        inPauseHud = false;
    }

    public void Resume()
    {
        for (int i = 0; i < pausingPlayObjects.Count; i++)
            pausingPlayObjects[i].Resume();

        ParticlesManager.Instance.ResumeParticles();
        AudioManager.instance.ResumeSounds();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Main.instance.GetChar().getInput.inMenu = false;
    }

    public void SettingsScreen()
    {
        mainButtons.SetActive(false);
        settingsHud.gameObject.SetActive(true);
        settingsHud.OpenGameplay();
        inSettings = true;
    }

    public void Cheats()
    {
        mainButtons.SetActive(false);
        cheatsHud.SetActive(true);
        Debug_UI_Tools.instance.Toggle(true);
    }

    public void BackToPause()
    {
        mainButtons.SetActive(true);
        Debug_UI_Tools.instance.Toggle(false);
        cheatsHud.SetActive(false);
        settingsHud.gameObject.SetActive(false);
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

        if (Input.GetButtonDown("Back"))
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

        if (Input.GetButtonDown("Pause") && canPress)
        {
            BackToPause();
            ResumeHud();
        }

        if (!canPress) canPress = true;
    }

    public void AddToPause(IPauseable po) => pausingPlayObjects.Add(po);
    public void RemoveToPause(IPauseable po) => pausingPlayObjects.Remove(po);
}
