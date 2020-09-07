using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpotCatcher : MonoBehaviour
{
    public Spot[] spots;
    private void Start()
    {
        spots = GetComponentsInChildren<Spot>();
        EquipedManager.instance.SetSpotsInTransforms(spots);
    }
}
