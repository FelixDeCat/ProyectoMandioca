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
                    current = misionesencola.Dequeue();
                    oneshot = true;
                    mensaje.MostrarMensaje(current.m, current.finalized);
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
                disparo = false;
                mensaje.CerrarMensaje(MensajeTerminoDeMostrarse);
                animate = false;
            }
        }
    }

    public void MensajeTerminoDeMostrarse()
    {
        disparo = true;
        oneshot = false;
        timer = 0;
    }

    public void LogearMision(Mision m, bool finalized, float duracion)
    {
        misionesencola.Enqueue(new Misionscomp(m, finalized, duracion));
        disparo = true;
    }

    public struct Misionscomp
    {
        public Mision m;
        public bool finalized;
        public float time;
        public Misionscomp(Mision _m, bool _finalized, float _time)
        {
            m = _m;
            finalized = _finalized;
            time = _time;

        }
    }
}
