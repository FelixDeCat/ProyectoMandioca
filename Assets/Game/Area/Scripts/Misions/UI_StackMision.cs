using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_StackMision : MonoBehaviour
{
    public static UI_StackMision instancia;

    public float timer;

    public UI_MisionMensaje mensaje;

    bool oneshot;
    bool animate;

    bool disparo;

    public Queue<Misionscomp> misionesencola = new Queue<Misionscomp>();
    private void Awake()
    {
        instancia = this;
    }

    Misionscomp current;
    public void Update()
    {
        if (disparo)
        {
            if (misionesencola.Count > 0)
            {
                if (!oneshot)
                {
                    Debug.LogWarning("Obtengo y muestro");
                    current = misionesencola.Dequeue();
                    oneshot = true;
                    mensaje.MostrarMensaje(current.m, current.tipo);
                    animate = true;
                }
            }
            else
            {
                disparo = false;
            }
        }

        if (animate)
        {
            if (timer < current.time)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                Debug.LogWarning("Termino el tiempo");
                disparo = false;
                mensaje.CerrarMensaje(MensajeTerminoDeMostrarse);
                animate = false;
            }
        }
    }

    public void MensajeTerminoDeMostrarse()
    {
        Debug.LogWarning("Termino de mostrar todo");
        disparo = true;
        oneshot = false;
        timer = 0;
    }

    public void LogearMision(Mision m, string tipo, float duracion)
    {
        misionesencola.Enqueue(new Misionscomp(m, tipo, duracion));
        disparo = true;
    }

    public struct Misionscomp
    {
        public Mision m;
        public string tipo;
        public float time;
        public Misionscomp(Mision _m, string _tipo, float _time)
        {
            m = _m;
            tipo = _tipo;
            time = _time;

        }
    }
}
