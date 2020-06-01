using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public static GameLoop instance; private void Awake() => instance = this;

    [SerializeField] private AudioClip ambience;
    
    Checkpoint_Manager checkpointmanager;
    public void SubscribeCheckpoint(Checkpoint_Manager checkpointmanager) => this.checkpointmanager = checkpointmanager;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_DEATH, CharacterIsDeath);
    }

    public void StartGame()
    {
       
    }
    /*
    play()
    pause()
    stop()
    respaw()
    */
    public void CharacterIsDeath()
    {
        //Main.instance.GetCombatDirector().RemoveTarget(Main.instance.GetChar());
        //character can´t receive damage
        //character deactivate
        //anim death
        //anim transition enter
        //calculate
        //anim transition exit
        //anim resurrect
        //character anim can´t receive damage
        //character activate
        //bla bla bla bla bla

        Invoke("CharacterResurrect", 0.5f);
        
        StartSoundAmbience();
    }

    void StartSoundAmbience()
    {
        AudioManager.instance.GetSoundPool("ambiente", AudioGroups.MUSIC, ambience);
        AudioManager.instance.PlaySound("ambiente");
    }

    public void CharacterResurrect()
    {
        //Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar());
        Main.instance.GetChar().Life.Heal_AllHealth();
        checkpointmanager.SpawnChar();
    }
}
