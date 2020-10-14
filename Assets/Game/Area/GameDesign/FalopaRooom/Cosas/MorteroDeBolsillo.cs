using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class MorteroDeBolsillo : MonoBehaviour
{
    public EventCounterPredicate predicate;

    CharacterHead character;

    //Ejemplo de predicado custom
    bool mi_espada_ya_tiene_untado_este_aceite = true;
    public bool MyPredicate() => mi_espada_ya_tiene_untado_este_aceite;
    private void Start() => predicate.Invoke(MyPredicate);

    //funciones del equipable/usable
    public void OnEquip() => character = Main.instance.GetChar(); //aca si queres puede ir una PASIVA
    public void OnUnequip() => Debug.Log("GreekOil=> OnUnEquip"); //aca si queres puede ir una PASIVA
    public void OnUpdateEquip() => Debug.Log("GreekOil=> OnUpdateEquip"); //aca si queres puede ir una PASIVA
    public void OnPress() => Debug.Log("GreekOil=> OnPress"); // presiono tecla
    public void OnRelease() => Debug.Log("GreekOil=> OnRelease"); // la suelto
    public void OnUpdateUse() => Debug.Log("GreekOil=> OnUpdateUse"); // se updatea mientras la tengo apretada
    public void OnExecute() => Createitem(); // El Uso mero mero

    public void Createitem()
    {

    }
}
