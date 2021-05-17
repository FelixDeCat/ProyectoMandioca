using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fastInfo : MonoBehaviour
{
    public TextMeshProUGUI info;

    private void Start()
    {
        info.text = transform.parent.GetComponent<InteractableTeleport>().informacion_del_teleport;
    }
}
