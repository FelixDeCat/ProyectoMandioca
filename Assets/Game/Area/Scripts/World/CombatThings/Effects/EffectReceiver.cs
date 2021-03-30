using System.Collections.Generic;
using UnityEngine;

public enum EffectName { OnFire, OnPetrify, OnFreeze, OnRoot, OnElectrified, OnPoison }

public class EffectReceiver : MonoBehaviour
{
    //Diccionario con todos los efectos posibles que tiene la entidad
    Dictionary<EffectName, EffectBase> myPossibleEffects = new Dictionary<EffectName, EffectBase>();

    //Lista con los efectos activos en el momento que tiene la entidad
    List<EffectName> activesEffect = new List<EffectName>();

    //Función que agrega un posible efecto al diccionario. Por ahora sólo es usado en el start, pero se puede usar para agregar invulnerabilidad momentánea o cosas así
    public void AddEffect(EffectName name, EffectBase effect)
    {
        if (!myPossibleEffects.ContainsKey(name))    
            myPossibleEffects.Add(name, effect);
        else
            Debug.LogError("Hay dos Efectos del mismo tipo en un mismo entity");
    }

    //Función que remueve un efecto del diccionario. Por ahora no se usa, pero lo mismo que arriba, se puede usar más adelante para jugar con las invulnerabilidades
    public void RemoveEffect(EffectName name) 
    {
        if (myPossibleEffects.ContainsKey(name))
        {
            myPossibleEffects[name].OnEndEffect();
            myPossibleEffects.Remove(name);
        }
    }

    //Cuando un efecto termina, llama a esta función para eliminarse de los efectos que están activos
    public void RemoveToActive(EffectName name)
    {
        if (activesEffect.Contains(name))
        {
            activesEffect.Remove(name);
            myPossibleEffects[name].OnEndEffect();
        }
    }

    //Función que se debe llamar para activar un efecto
    public void TakeEffect(EffectName effect, float cd = -1)
    {
        //Si mi EffectReceiver contiene el efecto que me pasaron, compruebo si ese efecto no está en marcha y si es compatible con los efectos actualmente activos. 
        //De ser así, ejecuto en efecto. Si el efecto no se encuentra, activo mi feedback de invulnerabilidad.
        if (myPossibleEffects.ContainsKey(effect))
        {
            if (activesEffect.Contains(effect)) return;
            for (int i = 0; i < activesEffect.Count; i++)
                if (myPossibleEffects[effect].incompatibilities.Contains(activesEffect[i])) return;
            
            myPossibleEffects[effect].OnStartEffect(cd);
            activesEffect.Add(effect);
        }
        else
        {
            Debug.Log("Feedback de inmunidad");
            //Por agregar
        }
    }

    //Esto se puede llamar cuando muero por ejemplo, para desactivar todos los efectos de una.
    public void EndAllEffects()
    {
        foreach (var item in myPossibleEffects)
            item.Value.OnEndEffect();
    }

    public void ExtendEffectDuration(EffectName effect, float timeExtend = -1)
    {
        if (activesEffect.Contains(effect))
        {
            myPossibleEffects[effect].ResetEffect(timeExtend);
        }
    }

    public void UpdateStates()
    {
        for (int i = 0; i < activesEffect.Count; i++) myPossibleEffects[activesEffect[i]].OnUpdate();
    }
}
