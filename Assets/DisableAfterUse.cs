using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterUse : MonoBehaviour
{
    public void Desactivar()
    {
      gameObject.SetActive(false);
    }
}
