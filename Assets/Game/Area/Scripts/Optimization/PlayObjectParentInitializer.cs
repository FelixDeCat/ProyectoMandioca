using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayObjectParentInitializer : LoadComponent
{
    [SerializeField] bool refreshChilds = false;
    [SerializeField] PlayObject[] myPlayObjects = new PlayObject[0];

    bool AlreadyProcessed = false;

    protected void Start() // por si mi parent no viene con Loader
    {
        if (!AlreadyProcessed && Application.isPlaying && myPlayObjects.Length>0)
        {
            AlreadyProcessed = true;
            StartCoroutine(Process());
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (refreshChilds)
        {
            refreshChilds = false;
            myPlayObjects = GetComponentsInChildren<PlayObject>();
        }
    }
#endif

    protected override IEnumerator LoadMe()//por si mi parent me llamó a cargar
    {
        Debug.Log("me llama?");
        if (!AlreadyProcessed)
        {
            yield return Process();
        }
        else yield return null;
    }

    IEnumerator Process()
    {
        Debug.Log(gameObject.scene.name);
        NewSceneStreamer.instance.AddToInitializers(gameObject.scene.name, this);
        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            myPlayObjects[i].Initialize();
            myPlayObjects[i].On();
            yield return null;
        }
    }

    public IEnumerator UnloadPlayObject()
    {
        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            if (myPlayObjects[i] != null && myPlayObjects[i].gameObject.activeSelf)
            {
                //apago esto un toque para probar algo
                //myPlayObjects[i].Off();
            }
            yield return null;
        }
    }
}