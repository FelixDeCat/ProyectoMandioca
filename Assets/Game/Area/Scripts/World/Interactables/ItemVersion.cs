using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemVersion : MonoBehaviour
{
    public GameObject worldVersion;
    public GameObject equipedVersion;
    public void Activate_WorldVersion()
    {
        worldVersion.SetActive(true);
        equipedVersion.SetActive(false);
    }
    public void Activate_EquipedVersion()
    {
        worldVersion.SetActive(false);
        equipedVersion.SetActive(true);
    }

    public GameObject GetEquipedVersion() => equipedVersion;
}
