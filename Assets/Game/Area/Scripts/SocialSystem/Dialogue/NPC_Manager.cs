using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NPC_Manager : LoadComponent
{
    public string sceneName = "";
    [SerializeField] NPCBase[] npcs = new NPCBase[0];

    bool AlreadyProcessed = false;

    [Header("ExecuteInEditMode")]
    public bool ButtonRefresh = true;

    protected void Start() // por si mi parent no viene con Loader
    {
        if (!AlreadyProcessed && Application.isPlaying)
        {
            AlreadyProcessed = true;
            StartCoroutine(Process());
        }
    }

    public void Test()
    {
        AlreadyProcessed = true;
        StartCoroutine(Process());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (ButtonRefresh)
        {
            ButtonRefresh = false;
            npcs = GetComponentsInChildren<NPCBase>();
        }
    }
#endif

    protected override IEnumerator LoadMe()//por si mi parent me llamó a cargar
    {
        if (!AlreadyProcessed)
        {
            yield return Process();
        }
        else yield return null;
    }

    IEnumerator Process()
    {
        for (int i = 0; i < npcs.Length; i++)
        {
            npcs[i].Initialize();
            yield return null;
        }
    }
}
