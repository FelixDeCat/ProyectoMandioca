using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DungeonGenerator.Core;

namespace DungeonGenerator.Components
{

    public class Room : MonoBehaviour
    {

        [SerializeField] Wing[] wings =  null;

        List<Room> vecinos = new List<Room>();
        Room myParent;

        [Header("Debug para saber en que room estoy parado")]
        public TextMesh infoRoom;

        [Header("(RoomTrigger) Grupo de Triggers")]
        public LocalRoomTriggers myRoomtrigger;

        int indexDir;
        int saveDir;
        int Xpos, Zpos;

        private void Awake()
        {
            myRoomtrigger = GetComponentInChildren<LocalRoomTriggers>();
        }

        public void CrearRoom(int _Xpos, int _Zpos, Room _parent)
        {
            Xpos = _Xpos;
            Zpos = _Zpos;
            myParent = _parent;
            this.transform.localPosition = new Vector3(Xpos, 0, Zpos);
            InstanciarNuevoHijo();
        }
        GameObject GetRoom()
        {
            GameObject go = new GameObject();
            go = Instantiate(ManagerRooms.instancia.GetPosibleRoom(ManagerRooms.instancia.indexLevel - 1));
            return go;
        }
        void InstanciarNuevoHijo()
        {
            indexDir = -1;
            Vector3 posChild = FindNextAvaliablePosition();

            if (posChild != Constantes.empty)
            {
                ManagerRooms.instancia.visitados[(int)posChild.x, (int)posChild.z] = true;
                ManagerRooms.instancia.indexLevel++;
                GameObject go = GetRoom();
                go.transform.SetParent(ManagerRooms.instancia.Parent);
                Room child = go.gameObject.GetComponent<Room>();
                if(child.infoRoom) child.infoRoom.text = ManagerRooms.instancia.indexLevel.ToString();
                ManagerRooms.instancia.AddRoom(child);
                //foreach (Transform t in child.posicionesContenedores)
                //    LevelConfig.instancia.InstanciarContenedor(t, child, LevelConfig.instancia.GetIndexLevelByCursor(ManagerRooms.instancia.indexLevel));
                this.ApagarWing(saveDir);
                child.ApagarWing(Constantes.GetOpuesto(saveDir));
                child.CrearRoom((int)posChild.x, (int)posChild.z, this);
            }
            else
            {
                if (myParent != ManagerRooms.instancia.roomBase) myParent.InstanciarNuevoHijo();
            }
        }
        public Vector3 FindNextAvaliablePosition()
        {
            saveDir = -1;
            List<int> indexs = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int movX = Xpos + Constantes.dirX[i];
                int movZ = Zpos + Constantes.dirZ[i];
                if (movX > -1 && movX <= ManagerRooms.instancia.sizeX - 1 && movZ > -1 && movZ <= ManagerRooms.instancia.sizeZ - 1)
                {
                    if (!ManagerRooms.instancia.visitados[movX, movZ]) indexs.Add(i);
                }
            }

            if (indexs.Count != 0)
            {
                indexDir = UnityEngine.Random.Range(0, indexs.Count);
                Vector3 result = new Vector3(Xpos + Constantes.dirX[indexs[indexDir]], 0, Zpos + Constantes.dirZ[indexs[indexDir]]);
                saveDir = indexs[indexDir];
                return result;
            }
            else return Constantes.empty;
        }
        public void SetVecino(Room vecino) { vecinos.Add(vecino); }
        public void PrenderWing(int i) { wings[i].IsActive = false; }
        public void ApagarWing(int i) { wings[i].IsActive = true; }
    }
}
