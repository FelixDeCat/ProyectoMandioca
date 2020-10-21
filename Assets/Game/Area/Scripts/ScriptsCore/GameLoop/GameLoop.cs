using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DevelopTools.UI;
using System;

public class GameLoop : MonoBehaviour
{
    public static GameLoop instance; private void Awake() => instance = this;

    private bool godMode = false;
    CharacterHead character;

    bool have_caronte;
    bool caronteIsActive = false;
    public void DEBUG_FORCE_ActivateCaronte() => have_caronte = true;
    public void DEBUG_FORCE_DeactivateCaronte() => have_caronte = false;

    #region [GO / BACK] Caronte Hell EVENTS
    public event Action OnGoToHell = delegate { };
    public event Action OnBackToHell = delegate { };
    public void ADD_EVENT_GoToHell(Action _callback) { OnGoToHell += _callback; }
    public void REMOVE_EVENT_GoToHell(Action _callback) { OnGoToHell -= _callback; }
    public void ADD_EVENT_BackFromHell(Action _callback) { OnBackToHell += _callback; }
    public void REMOVE_EVENT_BackFromHell(Action _callback) { OnBackToHell += _callback; }
    #endregion

    bool startGame;
    void Start()
    {
        character = Main.instance.GetChar();
        character.Life.ADD_EVENT_Death(OnPlayerDeath);
        TooglesConfig();
        startGame = true;
    }

    void OnPlayerDeath()
    {
        if (!godMode)
        {
            if (have_caronte)
            {
                if (!caronteIsActive)
                {
                    CaronteSecondChance();
                }
                else
                {
                    RealDeath();
                }
            }
            else
            {
                ChillDeath();
            }
        }
        else
        {
            character.Life.Heal_AllHealth();
        }
    }

    public void Resurrect(bool win_caronte_battle)
    {
        if (win_caronte_battle)
        {
            OnBackToHell.Invoke();
            //Fades_Screens.instance.Black();
            character.GetCharMove().SetSpeed();
            character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
            caronteIsActive = false;
            StartCoroutine(UnloadScene(win_caronte_battle));
        }
        else
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            /*
             *  ¿porqué copio exactamente lo mismo acá si se puede ejecutar una sola vez?
             *  no lo sé, quizas me volví loco... o quizá quiera recordale algo a mi yo del futuro
             */
            /////////////////////////////////////////////////////////////////////////////////////////////
            OnBackToHell.Invoke();
            //Fades_Screens.instance.Black();
            character.GetCharMove().SetSpeed();
            character.Life.Heal_AllHealth();
            caronteIsActive = false;
            StartCoroutine(UnloadScene(win_caronte_battle));
            // :thinking:
            // por ahora voy a dejarlo asi, pero acá iría toda la cinematica de la mano que te agarra
            // con su fade correspondiente
        }
    }
    IEnumerator UnloadScene(bool win)
    {
        var operation = SceneManager.UnloadSceneAsync("Caronte");

        if (operation.progress < 0.9f)
        {
            yield return null;
        }

        //xq el win va invertido? porque si gano deberia ir al checkpoint mas cercano... y para el checkpoint mas cercano es [inportant = false]
        Checkpoint_Manager.instance.SpawnChar(!win);

        yield return operation;

    }


    #region Death Functions 
    void CaronteSecondChance()
    {
        OnGoToHell.Invoke();
        LoadSceneHandler.instance.QuitSceneFromLoaded("Caronte");
        LoadSceneHandler.instance.LoadAScene("Caronte", false, LoadSceneMode.Additive);
        caronteIsActive = true;
    }
    void RealDeath()
    {
        Resurrect(false);
    }
    void ChillDeath()
    {
        Invoke("FastResurrect", 0.5f);
    }
    #endregion


    public void FastResurrect()
    {
        Main.instance.GetChar().Life.Heal_AllHealth();
        Checkpoint_Manager.instance.SpawnChar();
    }


    #region Toggles [CARONTE / GODMODE]
    void TooglesConfig()
    {
        Debug_UI_Tools.instance.CreateToogle("GODMODE", false, ToogleDebug);
        Debug_UI_Tools.instance.CreateToogle("CARONTE", false, ToggleCaronte);
    }
    string ToggleCaronte(bool val)
    {
        have_caronte = !have_caronte;
        return "C:=> " + (val ? "ON" : "OFF");
    }
    string ToogleDebug(bool active)
    {
        godMode = active;
        return active ? "debug activado" : "debug desactivado";
    }
    #endregion


    private void Update()
    {
        if (!startGame) return;
        if (character.transform.position.y < -100)
        {
            Checkpoint_Manager.instance.SpawnChar();
        }
    }
}
