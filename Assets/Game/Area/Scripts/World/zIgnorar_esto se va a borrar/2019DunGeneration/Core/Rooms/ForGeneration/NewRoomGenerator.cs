using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Extensions;
using System.Linq;

public class NewRoomGenerator : MonoBehaviour
{
    public static NewRoomGenerator instancia;

    NewRoom roomBase;

    [System.NonSerialized] public DimensionSectorManager dimensionSectors;
    public int MaxRooms;
    [System.NonSerialized] public int currentCantRooms;

    [Header("para forzar rooms")]
    public bool canforcerooms;
    public List<GameObject> forcerooms;

    [Header("Primera y ultima")]
    public GameObject firstRoom;
    public GameObject lastRoom;

    [Header("Mejora de procedimiento")]
    public List<RoomSamples> samples;

    [Header("para branchs")]
    public bool canBranch = true;
    public int MaxRoomsBranch;
    public int currentCantRoomsBranch;

    public NewRoom currentroom;

    public void SetCurrentRoom(NewRoom room) => currentroom = room;
    public NewRoom GetCurrentRoom() => currentroom;


    [System.Serializable]
    public class RoomSamples
    {
        public GameObject model;

        [Header("Para que haya una sola room de este tipo")]
        public bool one_per_dungeon;

        [Header("Para que no se repita una al lado de la otra")]
        public int weight;

        public int originalIndex;

        public int IndexToAppear;
    }


    private void Awake()
    {
        instancia = this;

        dimensionSectors = GetComponent<DimensionSectorManager>();
        if (dimensionSectors == null) throw new Exception("### no tenes DimensionSectorManager!!");

        for (int i = 0; i < samples.Count; i++)
        {
            samples[i].originalIndex = i;
        }
    }

    public event Action<List<NewRoom>> OnEnd;
    public List<NewRoom> listrooms = new List<NewRoom>();

    public void Start()
    {
        drawgizmos = true;
    }

    public void Generate(Action<List<NewRoom>> callbackend)
    {
        OnReset();
        listrooms = new List<NewRoom>();
        OnEnd += callbackend;
        FirstRoom();
    }

    private void Update()
    {
        dimensionSectors.OnUpdate();
    }

    public void OnReset()
    {
        currentCantRooms = 0;
        currentCantRoomsBranch = 0;
        var roomdestroy = FindObjectsOfType<NewRoom>();
        for (int i = 0; i < roomdestroy.Length; i++)
        {
            Destroy(roomdestroy[i].gameObject);
        }
        dimensionSectors.ONReset();
    }

    bool drawgizmos;

    public void OnDrawGizmos()
    {
        if (!drawgizmos) return;

        Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
        var v1 = new Vector3(dimensionSectors.x_global_min, 1, dimensionSectors.z_global_min) + offset;
        var v2 = new Vector3(dimensionSectors.x_global_max, 1, dimensionSectors.z_global_max) + offset;
        int same = 1;

        Gizmos.DrawLine(new Vector3(v1.x, same, v1.z), new Vector3(v2.x, same, v1.z));
        Gizmos.DrawLine(new Vector3(v1.x, same, v1.z), new Vector3(v1.x, same, v2.z));

        Gizmos.DrawLine(new Vector3(v2.x, same, v2.z), new Vector3(v2.x, same, v1.z));
        Gizmos.DrawLine(new Vector3(v2.x, same, v2.z), new Vector3(v1.x, same, v2.z));

    }

    public void FirstRoom()
    {
        foreach (var s in samples)
        {
            if (s.one_per_dungeon)
            {
                s.weight = int.MaxValue;
            }
        }

        var room = NewRoom.SpawnAndGet(firstRoom);
        room.transform.SetParent(this.transform);
        room.transform.position = new Vector3(5, 0, 5);
        roomBase = room;
        currentCantRooms++;
        AddRangesFromRoom(room);
        listrooms.Add(room);
        room.CreateNewRoom(null);

        OnEnd(listrooms);
    }


    public void AddRangesFromRoom(NewRoom room)
    {
        foreach (var r in room.rangeCornerManager.ranges)
        {
            NewRoomGenerator.instancia.dimensionSectors.AddDimensionRange(r.dimensionSectorRange);
        }
    }

    /// COMPROBACIONES
    public bool AvaliablePlaces() => currentCantRooms < MaxRooms;
    public bool AvaliablePlacesBranch() => currentCantRoomsBranch < MaxRoomsBranch;
    public Stack<RoomSamples> GetStackOffRooms()
    {
        var col = new List<RoomSamples>(samples);

        col = col
            .Shuffle()
            .OrderByDescending(x => x.weight)
            .ToList();

        return new Stack<RoomSamples>(col);
    }

    public Stack<GameObject> GetForcedRooms() => new Stack<GameObject>(forcerooms);
}
