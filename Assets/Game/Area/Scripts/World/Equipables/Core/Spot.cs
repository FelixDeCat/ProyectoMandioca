using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//esto va a ir en el parent de donde vamos a instanciar el equipable
//lo vamos a necesitar para hacer las busquedas, ya que
public class Spot : MonoBehaviour
{
    public SpotType spotType;
    public Transform spotparent;
}
