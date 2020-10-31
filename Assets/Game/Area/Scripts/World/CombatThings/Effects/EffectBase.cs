using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EffectBase : MonoBehaviour
{
    [SerializeField] EffectName effectType = EffectName.OnFire; //El tipo de effecto del script. Esto sirve también para hacer variaciones en funcionalidad de los efectos
    public List<EffectName> incompatibilities = new List<EffectName>(); //Lista de incompatibilidades. Por ejemplo, si yo no quiero prenderme fuego cuando estoy petrificado
    [SerializeField] float cdBase = 5.0f; //Por si no pasan ningún cd por parámetro, el efecto tiene un cd base
    [SerializeField] float cdMultiplier = 1.0f; //Esto es más que nada por si se quiere que los distintos enemigos resistan mejor algún efecto. Si pasan el cd por parámetro
                                               //se multipica por este multiplier, pudiendo durar más o menos según la entidad.

    public bool IsActive { get; private set; } //booleano cuando se activa el efecto. Es propiedad por si se quiere llamar de otro script.
    float timer; //timer del CD
    float currentCD; //Variable necesaria para saber si estoy usando el CD base o un CD que me pasaron por parámetro

    Action StartEffect; //Se ejecuta al empezar el efecto
    Action EndEffect; //Ser ejecuta al terminar el efecto

    protected void Start() => OnInitialize(); //Esto no se si se inicializaría con Start, pero lo había hecho con PlayObject y quedaba feo (mucha herencia que no necesitaba)

    //Agarro mi receiver (si tengo) y me agrego a los posibles efectos. Ademas, sumo mis funciones nativas a los Action
    protected virtual void OnInitialize()
    {
        if (GetComponentInParent<EffectReceiver>()) GetComponentInParent<EffectReceiver>().AddEffect(effectType, this); else Debug.LogError("No hay un EffectReceiver");

        StartEffect += OnEffect;
        EndEffect += OffEffect;
    }

    //Por si quiero agregar funcionalidad al start del efecto desde otro lado (por ejemplo cambiar el estado en la StateMachine)
    public void AddStartCallback(Action _StartEffect) => StartEffect += _StartEffect;

    //Lo mismo que el de arriba pero en el end
    public void AddEndCallback(Action _EndCallback) => EndEffect += _EndCallback;

    //OnUpdate más que nada para el cooldown, pero también ejecuto una función Tick con herencia, por si se quiere agregar funcionalidad según el tiempo del efecto
    //(Por ejemplo, si quiero que a la mitad del efecto, vomiten arcoiris)
    public void OnUpdate()
    {
        if (IsActive)
        {
            timer += Time.deltaTime;

            OnTickEffect(timer / currentCD);

            if (timer >= currentCD)
                OnEndEffect();
        }
    }

    //Empieza el efecto, chequeo si no estoy prendido, veo si me llegó un cd por parámetro, acomódo mi cd, y llamo al start del efecto.
    public virtual void OnStartEffect(float cd)
    {
        if (!IsActive)
        {
            IsActive = true;
            currentCD = cd == -1 ? cdBase : cd;
            currentCD *= cdMultiplier;
            StartEffect?.Invoke();
        }
    }

    public void ResetEffect(float cd)
    {
        if (IsActive)
        {
            currentCD = cd == -1 ? currentCD : cd * cdMultiplier;
            timer = 0;
            ResetEffectFeedback();
        }
    }

    protected virtual void ResetEffectFeedback() { }

    //Función nativa donde debería ir toda, o la mayor parte de la funcionalidad. Esta función se llama cuando comienza el efecto.
    protected abstract void OnEffect();

    //Cuando termina el cooldown o se tiene que terminar forzadamente el efecto, se llama a esta función. Por las dudas, chequea si está activo, llama a su funcionalidad
    //correspondiente y se remueve a sí mismo de los efectos activos.
    public virtual void OnEndEffect()
    {
        if (IsActive)
        {
            IsActive = false;
            timer = 0;
            EndEffect?.Invoke();
            GetComponentInParent<EffectReceiver>()?.RemoveToActive(effectType);
        }
    }

    //Función nativa donde debería y toda o la mayor parte de funcionalidad cuando termina un efecto.
    protected abstract void OffEffect();

    //Función que se llama en el OnUpdate y se le pasa el porcentaje de tiempo que falta para que termine el cooldown.
    protected abstract void OnTickEffect(float cdPercent);

    //Si les interesa, pueden ver los efectos de ejemplo que hice: EffectBasicPetrify, EffectBasicOnFire y EffectBasicFreeze.

    //La idea de estos componentes, es ser prefabs para que haya mayor flexibilidad a la hora de editar (Sino, si estos efectos fuesen compartidos por 5 diferentes tipos
    //de enemigos, al querer cambiar una partícula, shader, variable, etc. sería una tremenda paja entrar a prefab de enemigo, por prefab de enemigo a cambiar la
    //susodicha variable)
}
