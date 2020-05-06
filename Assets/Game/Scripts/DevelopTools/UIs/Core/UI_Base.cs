using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_AnimBase))]
public abstract class UI_Base : MonoBehaviour
{
    [Header("UI_Base")]
    public GameObject firstToOpenMenu;
    [System.NonSerialized] public int idfinder;
    [System.NonSerialized] public bool isActive;

    UI_AnimBase anim;
    [SerializeField] protected GameObject parent;
    void Awake()
    {
        anim = GetComponent<UI_AnimBase>();
        if (anim == null) throw new System.Exception("No contiene un UI_AnimBase");
        else anim.AddCallbacks(OnEndOpenAnimation, EndCloseAnimation);
        OnAwake();
    }
    void Start() { OnStart(); parent.SetActive(false); }
    void EndCloseAnimation() => OnEndCloseAnimation();
    void Update() { OnUpdate(); }
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnEndOpenAnimation();
    protected abstract void OnEndCloseAnimation();
    protected abstract void OnUpdate();
    public abstract void Refresh();
    public void ConfigurateFirst(GameObject go) => firstToOpenMenu = go;

    public virtual void Open()
    {
        anim.Open();
        parent.SetActive(true);
        Refresh();
        isActive = true;
        if(firstToOpenMenu) Main.instance.GetMyEventSystem().Set_First(firstToOpenMenu.gameObject);
    }
    public virtual void Close()
    {
        anim.Close();
        parent.SetActive(false);
        Main.instance.GetMyEventSystem().DeselectGameObject();
    }
}
