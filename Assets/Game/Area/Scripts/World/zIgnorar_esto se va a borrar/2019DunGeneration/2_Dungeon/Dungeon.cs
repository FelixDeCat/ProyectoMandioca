using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGenerator;
using DungeonGenerator.Components;
using System.Linq;


// Script viejo... si esto se llega a usar... 
// hay que heredar de LocalScene y hacer los ajustes correspondientes... 
// esto ya es un re frankenstein
public class Dungeon : SceneMainBase
{
    public NewRoomGenerator newManagerRooms;

    protected override IEnumerator LoadMe()
    {
        // aprovechar el LoadMe y meter el Build de Dungeon procedural aca
        yield return null;
    }

    protected override void OnAwake()
    {
        
    }

    protected override void OnStart()
    {
       // Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_END_LOAD, OnInitialize);
    }

    public void OnInitialize()
    {
        Debug.Log("create rooms");
        newManagerRooms.Generate(OnEndDungeonGeneration);
    }

    public void NextDungeon() 
    { 
        //MessageScreen.instancia.ShowAMessage(MessageScreen.Scene.dungeon, MessageScreen.MessaggeType.changedungeon); 
    }

    public void OnEndDungeonGeneration(List<NewRoom> rooms)
    {

        //tiene que haber un spawnpoint dentro de la room cero
        //si no lo hay setea como spawn el centro de la primer room
        var spawn = rooms[0].GetComponentInChildren<SpawnPoint>();
        if (spawn != null)
        {
            Vector3 v = spawn.transform.position;
            spawn_point = spawn.transform;
            Main.instance.GetChar().gameObject.transform.position = new Vector3(v.x, 0, v.z);
        }
        else
        {
            spawn_point = rooms[0].transform;
            Main.instance.GetChar().gameObject.transform.position = new Vector3(rooms[0].transform.position.x, 0, rooms[0].transform.position.z);
        }

        //acá forzamos a la primer room a decirle... chee, tenes al char adentro, ejecuta los IRoomElement
        rooms[0].myRoomtrigger.IsInside(Main.instance.GetChar().gameObject);
        
        
        ManagerRoomTrigger.instancia.Initialize(rooms);

        //CompleteCameraController.instancia.InstantAjust();
    }

    

    //protected override void OnFadeBackEnded()
    //{
    //   // FindObjectsOfType<MapComponent>().ToList().ForEach(x => x.Activate());
        
    //}

    //protected override void OnFadeGoEnded()
    //{
    //  //  FindObjectsOfType<MapComponent>().ToList().ForEach(x => x.Deactivate());
    //  //  CompleteCameraController.instancia.ChangeToNormal();
    //}

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            newManagerRooms.Generate( OnEndDungeonGeneration);
        }

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    CharBrain.instancia.transform.position = new Vector3(newManagerRooms.listrooms[newManagerRooms.rooms.Count - 1].transform.position.x, 0, newManagerRooms.rooms[newManagerRooms.rooms.Count - 1].transform.position.z);
        //    newManagerRooms.listrooms[newManagerRooms.rooms.Count - 1].myRoomtrigger.IsInside(CharBrain.instancia.gameObject);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    CharBrain.instancia.transform.position = new Vector3(newManagerRooms.rooms[0].transform.position.x, 0, newManagerRooms.rooms[0].transform.position.z);
        //    newManagerRooms.listrooms[0].myRoomtrigger.IsInside(CharBrain.instancia.gameObject);
        //}
    }

    protected override void TeleportBug()
    {
       // CharBrain.instancia.transform.position = new Vector3(newManagerRooms.listrooms[0].transform.position.x, 0, newManagerRooms.listrooms[0].transform.position.z);
       // newManagerRooms.listrooms[0].myRoomtrigger.IsInside(CharBrain.instancia.gameObject);
       // CompleteCameraController.instancia.InstantAjust();
    }

    public void PlayerIsDead() { }
    public void PlayerIsAlive() { }

    public override void OnPlayerDeath()
    {
        newManagerRooms.listrooms.ForEach(x => x.PlayerIsDeath());
    }


    protected override void OnFade_GoBlack()
    {
       
    }

    protected override void OnFade_GoTransparent()
    {
        
    }

    protected override void OnStartGame()
    {
        
    }
}
