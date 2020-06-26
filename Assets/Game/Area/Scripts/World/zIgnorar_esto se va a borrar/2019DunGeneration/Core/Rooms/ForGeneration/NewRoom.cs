using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools.Extensions;
using System;

public class NewRoom : ZoneBase
{
    [Header("Seteos")]
   [NonSerialized] public RangeCornerManager rangeCornerManager;
    
    public CellCore cellcore;

    [Header("(RoomTrigger) Grupo de Triggers")]
    [NonSerialized] public LocalRoomTriggers myRoomtrigger;

    List<NewRoom> vecinos = new List<NewRoom>();
    NewRoom myParent;

    //public TextMesh inforoom;

    DoorManager doormanager;

    bool isABranchRoom;
    bool marktocontinue;

    //recordar toda la parte de triggers habilitados
    public override void OnInitialize()
    {
        rangeCornerManager = GetComponentInChildren<RangeCornerManager>();
        myRoomtrigger = GetComponentInChildren<LocalRoomTriggers>();
        doormanager = GetComponentInChildren<DoorManager>();
        //esto tiene que estar antes de "doormanager.FreeAllDoors();" porque tiene que buscar los trigger antes de que el doormanager me los apague

        myRoomtrigger.Initialize();
        doormanager.FreeAllDoors();
        
        RefreshValues();//esto no va es solo para probar que todo funca
    }


    public void RefreshValues()
    {
        //calculo distancias locales
        //ahora seteo las cosas a mano.. mas adelante estaria bueno que las busque
        //door . aca hago el calculo para saber la distancia entre las puertas y el nucleo, y se lo seteo a cada uno
        //ranges. aca hago el calculo para saber la distancia entre cada corner y el nucleo, y se lo seteo a cada uno
        doormanager.Initialize(cellcore.transform.position);
        rangeCornerManager.Initialize();
    }
    /*
    **Una vez puesto todo en el lugar con su parent y posición y myparentroom....
    * me fijo en cual de las puertas quiero avanzar
    * elijo una
    * busco una room para spawnear
    * Recorro todas las puertas
    * Pregunto si no está ocupada
    * Agarró esa posición y le sumo +1 a la dirección
    * Una vez tenga ese más uno tengo la posición de spawn de la puerta del vecino
    * Ahora busco en la lista de vecinos... Y filtro los vecinos que pasen el filtro de...
    * Tengo puerta contraria
    * Y mis rangos no interfieren con los que ya estan
    Hago cálculo de pesos
    Y devuelvo la rooms elegida
    La spawneo
    Le digo que se pocione en la pos de spawn + el valor de distancia de núcleo que ya se le fue seteado
    Antes de esto arrancar su initialize
    Una vez se confirmó está room agrego su rango a la lista de rangos
         */
    public void SetOcuppiedDoor(int index_door)
    {
        doormanager.doors[index_door].Occuppied = true;
    }
    public CellDoor FindADoorToExit()
    {
        var celldoors = doormanager.doors.Where(x => !x.Occuppied).ToList();
        //aca hago algoritmo de cuales son las mejores puertas
        //o la direccion preferida por porcentaje
        return celldoors[UnityEngine.Random.Range(0, celldoors.Count)];
    }
    public void PlayerEnterInTheRoom()
    {
        

    }
    public void PlayerIsDeath()
    {
        myRoomtrigger.PlayerIsDeath();
    }

    public void CreateNewRoom(NewRoom parent)
    {
        RefreshValues();

        //Mis puertas
        var stackOfDoors = new Stack<CellDoor>(doormanager.doors.Where(x => !x.Occuppied).ToList().Shuffle());

        // RECORRO PUERTAS
        while (stackOfDoors.Count > 0)
        {
            //reservo
            var popped_door = stackOfDoors.Pop();

            // ME FIJO SI HAY LUGAR

            if (!isABranchRoom)
            {
                if ((NewRoomGenerator.instancia.AvaliablePlaces()))
                {
                    if (NewRoomGenerator.instancia.canBranch)
                    {
                        if (NewRoomGenerator.instancia.currentCantRooms == 10 || NewRoomGenerator.instancia.currentCantRooms == 15 || NewRoomGenerator.instancia.currentCantRooms == 20)
                        {
                            marktocontinue = true;
                        }
                    }
                   
                    Explode(popped_door);
                }
            }

            if (marktocontinue)
            {
                if (stackOfDoors.Count <= 0)
                {
                    if(parent)parent.marktocontinue = true;
                    marktocontinue = false;
                    return;
                }
                else
                {
                    popped_door = stackOfDoors.Pop();
                    if (!ExplodeBranch(popped_door))
                    {
                        if (stackOfDoors.Count == 0)
                        {
                            if (parent) parent.marktocontinue = true;
                            marktocontinue = false;
                            return;
                        }
                        continue;
                    }
                }  
            }
            if (isABranchRoom)
            {
                if (stackOfDoors.Count > 0)
                {
                    popped_door = stackOfDoors.Pop();
                    if (NewRoomGenerator.instancia.AvaliablePlacesBranch())
                    {
                        if (!ExplodeBranch(popped_door))
                        {
                            continue;
                        }
                    }
                }
                NewRoomGenerator.instancia.currentCantRoomsBranch = 0;
                return;
            }

            #region unused
            //if (NewRoomGenerator.instancia.currentCantRooms == 10 || NewRoomGenerator.instancia.currentCantRooms == 20)
            //{
            //    if (stackOfDoors.Count > 0)
            //    {
            //        isNode = true;
            //        var popped_door_branch = stackOfDoors.Pop();

            //        if (Explode(popped_door_branch, true))
            //        {
            //            this.gameObject.name = "(NODE)Room_" + NewRoomGenerator.instancia.currentCantRoomsBranch.ToString("00");
            //            isNode = false;
            //        }
            //        else
            //        {
            //            isNode = false;
            //            parent.isNode = true;
            //        }
            //    }
            //    else
            //    {
            //        isNode = false;
            //        parent.isNode = true;
            //    }
            //}
            #endregion
        }
    }

    bool RandomRoom(CellDoor door)
    {
        foreach (var s in NewRoomGenerator.instancia.samples)
        {
            if (NewRoomGenerator.instancia.currentCantRooms == s.IndexToAppear)
            {
                s.weight = 0;
            }
        }

        var stackrooms = NewRoomGenerator.instancia.GetStackOffRooms();
        // RECORRO LAS ROOMS
        
        while (stackrooms.Count > 0)
        {
            var room = new NewRoom();
            bool last = false;
            var sample = stackrooms.Pop();

            if (NewRoomGenerator.instancia.currentCantRooms == NewRoomGenerator.instancia.MaxRooms - 1)
            {
                room = SpawnAndGet(NewRoomGenerator.instancia.lastRoom);
                last = true;
            }
            else
            {
                room = SpawnAndGet(sample.model);
            }

            int index = 0;

            if (ComprobarLugarDeSpawn(door, room, ref index))
            {
                vecinos.Add(room);
                NewRoomGenerator.instancia.AddRangesFromRoom(room);

                SetOcuppiedDoor(door.index);
                room.doormanager.doors[index].Occuppied = true;

                room.gameObject.name = "Room_" + NewRoomGenerator.instancia.currentCantRooms.ToString("00");


                NewRoomGenerator.instancia.currentCantRooms++;

                NewRoomGenerator.instancia.listrooms.Add(room);

                if (!last)
                {
                    NewRoomGenerator.instancia.samples[sample.originalIndex].weight++;

                    if (sample.one_per_dungeon)
                    {
                        sample.weight = int.MaxValue;
                    }
                }
                

                room.CreateNewRoom(this);

                return true;
            }
            else
            {
                Destroy(room.gameObject);
                continue;
            }
        }
        return false;
    }
    bool ForcedRoom(CellDoor door)
    {
        var room = new NewRoom();

        Debug.Log("tengo " + NewRoomGenerator.instancia.forcerooms.Count + " forced rooms");
        Debug.Log("El current es: " + NewRoomGenerator.instancia.currentCantRooms);

        GameObject trygo = NewRoomGenerator.instancia.forcerooms[NewRoomGenerator.instancia.currentCantRooms-1];

        if (NewRoomGenerator.instancia.currentCantRooms == NewRoomGenerator.instancia.MaxRooms)
        {
            room = SpawnAndGet(NewRoomGenerator.instancia.lastRoom);
        }
        else
        {
            room = SpawnAndGet(trygo);
        }

        int index = 0;
        if (ComprobarLugarDeSpawn(door, room, ref index))
        {
            vecinos.Add(room);
            NewRoomGenerator.instancia.AddRangesFromRoom(room);
            SetOcuppiedDoor(door.index);
            room.doormanager.doors[index].Occuppied = true;
            room.gameObject.name = "Room_" + NewRoomGenerator.instancia.currentCantRooms.ToString("00");
            NewRoomGenerator.instancia.currentCantRooms++;
            NewRoomGenerator.instancia.listrooms.Add(room);
            room.CreateNewRoom(this);
            return true;
        }
        else
        {
            Destroy(room.gameObject);
            return false;
        }
    }

    public bool Explode(CellDoor door)
    {
        if (NewRoomGenerator.instancia.canforcerooms)
        {
            return ForcedRoom(door);
        }
        else
        {
            return RandomRoom(door);
        }

        
    }
    public bool ExplodeBranch(CellDoor door)
    {
        
        var stackrooms = NewRoomGenerator.instancia.GetStackOffRooms();

        // RECORRO LAS ROOMS
        while (stackrooms.Count > 0)
        {
            var sample = stackrooms.Pop();

            var room = SpawnAndGet(sample.model);

            int index = 0;

            if (ComprobarLugarDeSpawn(door, room, ref index))
            {
                vecinos.Add(room);
                NewRoomGenerator.instancia.AddRangesFromRoom(room);

                SetOcuppiedDoor(door.index);
                room.doormanager.doors[index].Occuppied = true;
                room.gameObject.name = "--Room_" + NewRoomGenerator.instancia.currentCantRoomsBranch.ToString("00");
                Debug.Log("BRANCH_ " + NewRoomGenerator.instancia.currentCantRoomsBranch);
                room.isABranchRoom = true;

                NewRoomGenerator.instancia.currentCantRoomsBranch++;

                NewRoomGenerator.instancia.samples[sample.originalIndex].weight++;

                room.CreateNewRoom(this);

                return true;
            }
            else
            {
                Destroy(room.gameObject);
                continue;
            }
        }
        return false;
    }
    public static NewRoom SpawnAndGet(GameObject newRoom)
    {
        GameObject go = Instantiate(newRoom);
        var room = go.GetComponent<NewRoom>();
        room.Initialize();
        room.RefreshValues();
        return room;
    }
    public bool ComprobarLugarDeSpawn(CellDoor cellDoor, NewRoom room, ref int index)
    {
       
        //obtengo la direccion hacia donde tengo que ir
        //obtengo todas las puertas del vecino
        //agarro una al azar
        var dirtospawn = cellDoor.doorDir.V3ToSpawn();
        var puertasdelvecino = room.doormanager.GetDoorsByType(cellDoor.doorDir.Counter());
        if (puertasdelvecino.Count <= 0)
        {
           
            return false;
        }
        var currentdoor = puertasdelvecino[UnityEngine.Random.Range(0, puertasdelvecino.Count)];

        index = currentdoor.index;

        
        //calculo la posicion donde la proxima room tiene que spawnear
        //lo posiciono
        //actualizo los valores
        var spawnPosition = new Vector3(
            cellDoor.transform.position.x + dirtospawn.x + currentdoor.x_localdistance_tocore,
            0,
            cellDoor.transform.position.z + dirtospawn.z + currentdoor.z_localdistance_tocore);
        room.transform.position = spawnPosition;
        room.transform.SetParent(NewRoomGenerator.instancia.gameObject.transform);
        room.RefreshValues();

        //me fijo si una vez posicionado realmente puedo posicionarlo ahi
        //
        foreach (var d in room.rangeCornerManager.ranges)
        {
            if (!NewRoomGenerator.instancia.dimensionSectors.CanAddDimensionRange(d.dimensionSectorRange))
            {
                Destroy(room.gameObject);
                return false;
            }
            else
            {
                continue;
            }
        }

        //esto lo agrego a la lista de dimensiones, tiene que ir afuera
        //porque me tengo que asegurar de que todas sean validas primero

        return true;
    }

    
}


