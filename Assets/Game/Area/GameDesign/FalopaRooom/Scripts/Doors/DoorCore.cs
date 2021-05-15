using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorCore : MonoBehaviour
{
    bool open = false;
    bool isAnim = false;
    bool animate_open = false;
    float timer = 0;
    [Header("Core Door Options")]
    [SerializeField] float time_to_open = 2.0f;
    [SerializeField] float time_to_close = 2.0f;

    enum animtype { Classic_Door, Other };
    [SerializeField] animtype animation_type = animtype.Classic_Door;

    /// <summary> INVIERTE (si me ejecutaron dos veces lo hace otra vez pero al revez) </summary>
    public void Switch()
    {
        if (!isAnim && CustomCondition())
        {
            animate_open = !open;
            isAnim = true;
            timer = 0;
        }
        else
        {
            Feedback_On_Execution_Failed();
        }
    }

    /// <summary> SOLO ABRE (si me ejecutaron dos veces no hace nada) </summary>
    public void Open()
    {
        if (!isAnim && !open && CustomCondition())
        {
            animate_open = true;
            isAnim = true;
            timer = 0;
        }
        else
        {
            Feedback_On_Execution_Failed();
        }
    }

    /// <summary> SOLO CIERRA (si me ejecutaron dos veces no hace nada) </summary>
    public void Close()
    {
        if (!isAnim && open && CustomCondition())
        {
            animate_open = false;
            isAnim = true;
            timer = 0;
        }
        else
        {
            Feedback_On_Execution_Failed();
        }
    }

    protected abstract bool CustomCondition();
    protected abstract void LerpValue(float lerp_value);

    /// <summary> se ejecuta cuando empieza a abrirse </summary>
    protected abstract void Feedback_On_Begin_Open();
    protected abstract void Feedback_On_End_Open();
    protected abstract void Feedback_On_Begin_Close();
    protected abstract void Feedback_On_End_Close();
    protected abstract void Feedback_On_Execution_Failed();

    void Animate(float val) => LerpValue(val);

    private void Update()
    {
        if (!isAnim) return;

        switch (animation_type)
        {
            case animtype.Classic_Door:

                float MAX = animate_open ? time_to_open : time_to_close; // ¿ que máximo voy a usar ?

                if (timer < MAX)
                {
                    timer = timer + 1 * Time.deltaTime;

                    float normalized_direction = 
                        animate_open ?          // ¿ estoy abriendo ?
                        timer / MAX :           // voy de 0 hasta 1
                        1 - (timer / MAX);      // voy de 1 hasta 0

                    Animate(normalized_direction);
                }
                else
                {
                    open = animate_open;                // lo seteo abierto o cerrado dependiendo de la animacion que me ejecutaron
                    Animate(animate_open ? 1 : 0);      // clamp por si las moscas
                    timer = 0;
                    isAnim = false;
                }
                
                break;

            case animtype.Other:

                //si quieren romper este código haganlo acá, ¡no me toquen la puerta clásica! >:c

                break;
        }
    }
}
