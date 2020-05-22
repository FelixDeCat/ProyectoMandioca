using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    static List<IUpdateble> allUpdatesObject = new List<IUpdateble>();

    public static void AddObjectUpdateable (IUpdateble objectUpdatable)
    {
        if(!allUpdatesObject.Contains(objectUpdatable))
            allUpdatesObject.Add(objectUpdatable);
	}

    public static void RemoveObjectUpdateable(IUpdateble objectUpdatable)
    {
        if(allUpdatesObject.Contains(objectUpdatable))
            allUpdatesObject.Remove(objectUpdatable);
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var updateable in allUpdatesObject)
        {
            updateable.OnUpdate();
        }

	}
}
