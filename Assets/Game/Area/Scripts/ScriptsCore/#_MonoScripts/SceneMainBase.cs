using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

// CLASE VIEJA... esto antes era un Monobehavior, 
// pero ahora tiene el overpower de PlayableScene como parent
// todavia no esta ordenado este script... hay que adaptarlo cuando haya tiempo
// este scrip podra ser hereadod de los Scripts Main locales de cada escena
// o sea... vamos a tener el Main que viene desde arriba con su DontDestroy y su propio
// script de carga... entonces este script que esta acá servira como HandRail de
// cargas pesadas y Builds de objetos locales
// a parte que tiene todas las funciones de Playables clasicas de un videojuego como play, resume, pause, etc

public abstract class SceneMainBase : PlayableScene
{
    [SerializeField] UnityEvent InputCheckHasCore;
    [SerializeField] UnityEvent InputSpawn;

    private void Awake() 
    { 
        OnAwake(); 
    }
    protected abstract void OnAwake();

    private void Start() 
    { 
        OnStart(); 
    }
    protected abstract void OnStart();


    protected virtual void OnExitEscene() { }
    

    [System.Obsolete("luego se va a armar un handler para fades, esto tiene que estar en un component")]
    protected abstract void OnFade_GoBlack();
    [System.Obsolete("luego se va a armar un handler para fades, esto tiene que estar en un component")]
    protected abstract void OnFade_GoTransparent();

    private void Spawn() //el spawn lo usaba para cargar los datos de esa escena, posicionar el character, la camara, etc etc etc
    {

    }

    //hay que crear un OnPlayerBuggedParameters.cs
    //en esta clase vamos a harcodear tooooooodas
    //las condiciones y parches para que me avisen
    //a esta funcion que el character se bugeo en
    //algun lado... luego esto derivarlo al spawner
    protected virtual void TeleportBug()
    {

    }

    
}
