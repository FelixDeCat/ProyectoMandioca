using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// esta es la estructura que llevaria los scripts de main propios de cada escena

public class LocalScene : SceneMainBase //  --->>  / SceneMainBase > PlayableScene > LoadComponent
{
    // para getters de referencias y set de variables locales
    // [aca se puede poner Finders o GetComponents]
    protected override void OnAwake() { }

    //start de la linea del monovehaviour, por si las moscas
    protected override void OnStart() { }

    //Update que se ejecuta una vez comienza el juego, cuando esta en pausa tampoco se ejecuta
    protected override void OnUpdate() { }

    // para carga asincrónica, aca va todo lo que vaya a ser pesado
    // [aca por ejemplo si hay que instanciar cosas, poner algun algoritmo de exploracion lookuptable o si por si queremos hacer un stream object desde archivo]
    protected override IEnumerator LoadMe() { yield return null; }

    //se ejecuta cuando el termina la carga local y la animacion de entrada (que aun no tenemos XD)
    protected override void OnStartGame()
    {

    }

    //para los events de Fade... esto no me convence mucho que este aca... tal vez haga un component para Fade
    [System.Obsolete("luego se va a armar un handler para fades, esto tiene que estar en un component")]
    protected override void OnFade_GoBlack() { }
    [System.Obsolete("luego se va a armar un handler para fades, esto tiene que estar en un component")]
    protected override void OnFade_GoTransparent() { }


    //esto todavia no funca... hay que linkearlo al event manager... viene de PlayableScene
    public override void OnPlayerDeath() { }
    

    
}
