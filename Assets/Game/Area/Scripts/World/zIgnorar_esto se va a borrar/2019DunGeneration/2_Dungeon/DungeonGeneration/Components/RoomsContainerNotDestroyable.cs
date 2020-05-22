using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator.Components
{
    public class RoomsContainerNotDestroyable : MonoBehaviour
    {
        bool marktodelete;

        List<Room> rooms = new List<Room>();
        List<NewRoom> newrooms = new List<NewRoom>();

        //public ParentContainer parentcontainer;

        public void MarkToDelete() { marktodelete = true; }
        public bool IsMarkedToDelete() { return marktodelete; }

        public List<Room> GetRooms() { return rooms; }
        public List<NewRoom> GetNewRooms() { return newrooms; }

        public void SetRooms(List<Room> col)
        {
            rooms = col;
        }
        public void SetRooms(List<NewRoom> col)
        {
            newrooms = col;
        }

        public bool ContainsRooms()
        {
            return GetComponentsInChildren<Room>().Length > 1;
        }
        public bool ContainsNewRooms()
        {
            return GetComponentsInChildren<NewRoom>().Length > 1;
        }

        public void Clean()
        {

          //  var childs = parentcontainer.gameObject.GetComponentsInChildren<ItemWorld>();

          //  for (int i = 0; i < childs.Length; i++)
          //  {
          //      Destroy(childs[i].gameObject);
          //  }

          //  for (int i = 0; i < rooms.Count; i++)
          //  {
          //      Destroy(rooms[i].gameObject);
          //  }
          //  rooms.Clear();
          //  marktodelete = false;
        }
        public void CleanNewRooms()
        {

          // var childs = parentcontainer.gameObject.GetComponentsInChildren<ItemWorld>();

          //  for (int i = 0; i < childs.Length; i++)
          //  {
          //      Destroy(childs[i].gameObject);
          //  }

          //  for (int i = 0; i < newrooms.Count; i++)
          //  {
          //      Destroy(rooms[i].gameObject);
          //  }
          //  rooms.Clear();
          //  marktodelete = false;
        }
    }
}

