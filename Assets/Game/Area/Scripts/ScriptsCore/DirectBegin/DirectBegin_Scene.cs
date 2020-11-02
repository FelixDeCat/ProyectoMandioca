﻿/////////////////////////////////////////////////////////////
/// Este script es para que si estas en la escena que querés
/// probar, detecte si pasaste primero por la carga.
/// pregunta si esta el Main, si no está... te lleva al Load.
/// Esto es basicamente para que no tengas que abrir el Load
/// cada vez que queres testear algo
/////////////////////////////////////////////////////////////
namespace Tools.Testing
{
    using UnityEngine;
    using System.Linq;
    using System.Collections;

    public class DirectBegin_Scene : MonoBehaviour
    {
        public bool LockMouse;
        [SerializeField] LocalPackageLoadComponent packageToLoad = null;

        [SerializeField] GameObject[] toIgnoreIfThisIsNotTheMoment = new GameObject[1];

        public void Awake()
        {
            if (Main.instance == null) // si entra acá es porque nunca entro a la escena de carga
            {
                for (int i = 0; i < toIgnoreIfThisIsNotTheMoment.Length; i++)
                {
                    DestroyImmediate(toIgnoreIfThisIsNotTheMoment[i].gameObject);
                }
                GameObject jumper = new GameObject();
                jumper.AddComponent<DirectBegin_Jumper>();
                jumper.GetComponent<DirectBegin_Jumper>().Configure(Scenes.GetActiveSceneName());
                Scenes.UnloadThisScene();
                Scenes.Load_Load();
            }
            else
            {
                Fades_Screens.instance.Black();
                Fades_Screens.instance.FadeOff(() => {});
                LoadSceneHandler.instance.Off_LoadScreen();
                Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);
                //Debug.Log("entro mas veces aca");
                if (LockMouse)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                packageToLoad?.LoadComponents();
                Destroy(this.gameObject);
            }
        }
    }
}
