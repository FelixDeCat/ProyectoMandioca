using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Boo.Lang;
using UnityEngine.UI;
using Tools;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    List<PlayObject> pausingPlayObjects = new List<PlayObject>();
    [SerializeField] UI_Anim_Code pauseHud = null;
    [SerializeField] GameObject backgroundPause = null;
    [SerializeField] GameObject mainFirstButton = null;
    [SerializeField] GameObject mainButtons = null;

    private void Awake()
    {
        Instance = this;

        pauseHud.AddCallbacks(() => { }, () => mainButtons.SetActive(false));
    }

    public void Pause()
    {
        for (int i = 0; i < pausingPlayObjects.Count; i++)
            pausingPlayObjects[i].Pause();

        ParticlesManager.Instance.PauseParticles();
        AudioManager.instance.PauseSounds();
        backgroundPause.SetActive(true);
        pauseHud.Open();
        mainButtons.SetActive(true);
        MyEventSystem.instance.SelectGameObject(mainFirstButton);
        Main.instance.GetChar().getInput.inMenu = true;
    }

    public void Resume()
    {
        for (int i = 0; i < pausingPlayObjects.Count; i++)
            pausingPlayObjects[i].Resume();

        ParticlesManager.Instance.ResumeParticles();
        AudioManager.instance.ResumeSounds();
        backgroundPause.gameObject.SetActive(false);
        pauseHud.Close();
        MyEventSystem.instance.SelectGameObject(null);
        Main.instance.GetChar().getInput.inMenu = false;
    }

    public void Settings()
    {

    }

    public void Cheats()
    {

    }

    public void BackToPause()
    {

    }

    public void ReturnToMenu()
    {
        var myGameCores = FindObjectsOfType<DontDestroy>().Where(x => x.transform != transform.parent).ToArray();

        for (int i = 0; i < myGameCores.Length; i++)
            Destroy(myGameCores[i].gameObject);
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(transform.parent.gameObject);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.O))
    //        ReturnToMenu();
    //}

    public void AddToPause(PlayObject po) => pausingPlayObjects.Add(po);
    public void RemoveToPause(PlayObject po) => pausingPlayObjects.Remove(po);
}
