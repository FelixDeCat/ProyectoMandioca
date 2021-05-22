using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayObjectParentInitializer : MonoBehaviour
{
    PlayObject[] myPlayObjects = new PlayObject[0];

    bool isInitialized;

    private void Start()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            myPlayObjects = GetComponentsInChildren<PlayObject>();

            for (int i = 0; i < myPlayObjects.Length; i++)
            {
                myPlayObjects[i].Initialize();
                myPlayObjects[i].On();
            }
        }
    }

    void OnEnable()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            myPlayObjects = GetComponentsInChildren<PlayObject>();

            for (int i = 0; i < myPlayObjects.Length; i++)
            {
                myPlayObjects[i].Initialize();
                myPlayObjects[i].On();
            }
        }
    }
    void OnDisable()
    {
        if (isInitialized)
        {
            isInitialized = false;
            myPlayObjects = GetComponentsInChildren<PlayObject>();

            for (int i = 0; i < myPlayObjects.Length; i++)
            {
                myPlayObjects[i].Deinitialize();
                myPlayObjects[i].Off();
            }
        }
    }
    private void OnDestroy()
    {
        if (isInitialized)
        {
            isInitialized = false;
            myPlayObjects = GetComponentsInChildren<PlayObject>();

            for (int i = 0; i < myPlayObjects.Length; i++)
            {
                myPlayObjects[i].Deinitialize();
                myPlayObjects[i].Off();
            }
        }
    }
}