using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instancia;
    private void Awake() { instancia = this; }


    public void GenerateSoundWave(Vector3 position, float radius, ZoneBase room)
    {
        //var enems = room.FindInvestigatorsInRadius(position, radius);

        //foreach (var e in enems)
        //{
        //    e.OnSoundListen(position);
        //}
    }
    public void GenerateSoundWave(Inspectable insp, float radius, ZoneBase room)
    {
        //Debug.Log("Insp", insp);
        //Debug.Log("Roombase", room);

        ////var enems = room.FindInvestigatorsInRadius(insp.posToInspect.position, radius);

        //Debug.Log(enems.Count);

        //enems[Random.Range(0, enems.Count)].OnSoundListen(insp);
    }
}
