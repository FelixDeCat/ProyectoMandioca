using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWall_controller : MonoBehaviour
{
    public List<TentacleWall> tentacles = new List<TentacleWall>();

    private void Start()
    {
        foreach (Transform item in transform)
        {
            var tentacle = item.GetComponent<TentacleWall>();

            if (tentacle != null)
                tentacles.Add(tentacle);
        }        
    }

    public void OpenTentacles()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].gameObject.SetActive(true);
        }
    }

    public void CloseTentacles()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].CloseTentacles();
        }
    }
}
