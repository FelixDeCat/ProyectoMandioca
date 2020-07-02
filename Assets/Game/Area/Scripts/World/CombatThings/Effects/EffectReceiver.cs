using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName { OnFire, OnPetrify, OnFreeze }

public class EffectReceiver : MonoBehaviour
{
    Dictionary<EffectName, EffectBase> myPossibleEffects = new Dictionary<EffectName, EffectBase>();

    public void AddEffect(EffectName name, EffectBase effect)
    {
        if (!myPossibleEffects.ContainsKey(name))
            myPossibleEffects.Add(name, effect);
        else
            Debug.LogError("Hay dos Efectos del mismo tipo en un mismo entity");
    }

    public void RemoveEffect(EffectName name)
    {
        if (myPossibleEffects.ContainsKey(name))
            myPossibleEffects.Remove(name);
    }

    public void TakeEffect(EffectName effect, float cd = -1)
    {
        if (myPossibleEffects.ContainsKey(effect))
        {
            myPossibleEffects[effect].OnStartEffect(cd);
        }
        else
        {
            Debug.Log("Feedback de inmunidad");
            //Por agregar
        }
    }

    public void EndAllEffects()
    {
        foreach (var item in myPossibleEffects)
            item.Value.OnEndEffect();
    }

}
