using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSword_animHelper : MonoBehaviour
{
    public void OnFinishAnimation()
    {
        gameObject.SetActive(false);
    }
}
