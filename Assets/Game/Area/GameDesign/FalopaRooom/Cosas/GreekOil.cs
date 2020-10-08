using System.Collections;
using System.Collections.Generic;
using Tools.EventClasses;
using UnityEngine;

public class GreekOil : MonoBehaviour
{
    public EventCounterPredicate predicate;

    //Ejemplo de predicado custom
    bool mi_espada_ya_tiene_untado_este_aceite = true;
    public bool MyPredicate() => mi_espada_ya_tiene_untado_este_aceite;
    private void Start() => predicate.Invoke(MyPredicate);

    //funciones del equipable/usable
    public void OnEquip() => Debug.Log("GreekOil=> OnEquip"); //aca si queres puede ir una PASIVA
    public void OnUnequip() => Debug.Log("GreekOil=> OnUnEquip"); //aca si queres puede ir una PASIVA
    public void OnUpdateEquip() => Debug.Log("GreekOil=> OnUpdateEquip"); //aca si queres puede ir una PASIVA
    public void OnPress() => Debug.Log("GreekOil=> OnPress"); // presiono tecla
    public void OnRelease() => Debug.Log("GreekOil=> OnRelease"); // la suelto
    public void OnUpdateUse() => Debug.Log("GreekOil=> OnUpdateUse"); // se updatea mientras la tengo apretada
    public void OnExecute() => FireInTheSword(); //Debug.Log("GreekOil=> OnExecute"); // El Uso mero mero


    public void FireInTheSword()
    {
        Main.instance.GetChar().GetCharacterAttack().Add_callback_SecondaryEffect(EscribirAlgo);
    }

    void EscribirAlgo(EffectReceiver alQueAfecto)
    {
        Debug.Log(alQueAfecto.name);
    }
}
