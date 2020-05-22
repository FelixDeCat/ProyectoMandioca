using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DungeonGenerator.Components;


namespace DungeonGenerator
{
    public class ManagerRooms : MonoBehaviour
    {
        public static ManagerRooms instancia;

        [System.NonSerialized] public List<Room> rooms = new List<Room>();
        [System.NonSerialized] public Room roomBase;
        public bool[,] visitados;
        public Transform Parent { get; private set; }
        int posx, posz;

        public event Action<List<Room>> callbackrooms;

        public int indexLevel;
        [Header("Gold en esta Dungeon")]
        public int cantidadDeGoldTotal;
        public GameObject modelgold;

        [Header("Others")]
        public RoomsAndNodes roomsAndNodes;

        [Header("GD: la escala que van a tener las rooms")]
        public float multiplyScaleX = 12;
        public float multiplyScaleZ = 10;
        [Header("GD: cuantas rooms x cuantas rooms")]
        public int sizeX = 10;
        public int sizeZ = 10;
        [Header("GD: Posibles rooms")]
        public PosibleRooms[] posibles;

        public void AddRoom(Room room) { rooms.Add(room); }

        void Awake()
        {
            instancia = this;
        }

        public void Generate(Transform parentRooms ,Action<List<Room>> callback)
        {
            Parent = parentRooms;
            foreach (Room r in rooms) { Destroy(r.gameObject); }
            indexLevel = 0;
            callbackrooms += callback;
            BuildRooms();
        }

        void End() { callbackrooms(rooms); }

        public GameObject GetPosibleRoom(int index) { return posibles[index].GetARoom(); }

        public void BuildRooms()
        {
            indexLevel++;
            visitados = new bool[sizeX, sizeZ];
            rooms = new List<Room>();
            posx = UnityEngine.Random.Range(0, sizeX);
            posz = UnityEngine.Random.Range(0, sizeZ);

            GameObject go = Instantiate(GetPosibleRoom(0));
            go.transform.SetParent(Parent);
            go.name = "Room Base";

            Room first = go.gameObject.GetComponent<Room>();

            if (first.infoRoom) first.infoRoom.text = "BASE";
            roomBase = first;
            rooms.Add(first);
            visitados[posx, posz] = true;
            first.CrearRoom(posx, posz, null);
            foreach (var r in rooms) r.transform.position = new Vector3(r.transform.position.x * multiplyScaleX, r.transform.position.y, r.transform.position.z * multiplyScaleZ);
            Invoke("End", 0.1f);
        }
    }
}


[System.Serializable]
public class PosibleRooms
{
    public GameObject[] PosiblesRooms;

    public GameObject GetARoom()
    {
        return PosiblesRooms[UnityEngine.Random.Range(0, PosiblesRooms.Length)];
    }
}
