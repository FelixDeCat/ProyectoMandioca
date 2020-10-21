using System.Collections;
using System.Collections.Generic;
using Tools.EventClasses;
using UnityEngine;

public class GreekOil : MonoBehaviour
{
    public EventCounterPredicate predicate;

    [SerializeField] float duration = 60;
    CharacterHead character;

    //Ejemplo de predicado custom
    bool mi_espada_ya_tiene_untado_este_aceite = true;
    public bool MyPredicate() => mi_espada_ya_tiene_untado_este_aceite;
    private void Start() => predicate.Invoke(MyPredicate);

    //funciones del equipable/usable
    public void OnEquip() => character = Main.instance.GetChar(); 
    public void OnUnequip() { } 
    public void OnUpdateEquip() { }
    public void OnPress() { } 
    public void OnRelease() { }
    public void OnUpdateUse() { } 
    public void OnExecute() { } 


    public void FireInTheSword()
    {
        character.TurnOnGreekOilEffect(duration);
        //StartCoroutine(StartEffectOnSword());
        //Debug.Log("ejecuto greek oil");
    }

    //IEnumerator StartEffectOnSword()
    //{
    //    character.GetCharacterAttack().Add_callback_SecondaryEffect(PrenderFuegoAEnemigo);

    //    character.TurnOnGreekOilEffect();
    //    float count = 0;

    //    while(count < duration)
    //    {
    //        count += Time.deltaTime;           

    //        yield return new WaitForEndOfFrame();
    //    }

    //    character.TurnOffGreekOilEffect();
    //    character.GetCharacterAttack().Remove_callback_SecondaryEffect(PrenderFuegoAEnemigo);
    //}

    //void PrenderFuegoAEnemigo(EffectReceiver alQueAfecto)
    //{
    //    alQueAfecto.TakeEffect(EffectName.OnFire);
    //}
}
