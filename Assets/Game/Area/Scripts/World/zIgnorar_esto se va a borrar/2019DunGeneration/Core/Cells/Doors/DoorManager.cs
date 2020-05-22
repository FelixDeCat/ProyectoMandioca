using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorManager : MonoBehaviour
{
    public List<CellDoor> doors;

    public void Initialize(Vector3 posCore)
    {
        //PARA COMPROBAR QUE ESTO FUNCIONA, este donde este la room siempre te va a dar los mismo valores

        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].index = i;
            
            doors[i].x_localdistance_tocore = (int)(posCore.x - doors[i].transform.position.x);
            doors[i].z_localdistance_tocore = (int)(posCore.z - doors[i].transform.position.z);
        }
    }

    public void FreeAllDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].Occuppied = false;
        }
    }

    public void Turn_ON_Door(int i) => doors[i].TurnOn();
    public void Turn_OFF_Door(int i) => doors[i].TurnOff();

    public List<CellDoor> GetDoorsByType(int i)
    {
        return doors.Where(x => x.doorDir == i && !x.Occuppied).ToList();
    }
}
