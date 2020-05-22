using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManagerPlayObjects : MonoBehaviour
{
    private void Awake()
    {
        FindObjectsOfType<EntityBase>().ToList().ForEach(x => x.On());
    }
}
