using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunManager : MonoBehaviour
{
    /////////////////////////
    /// EXTERNO
    /////////////////////////
    float timer;
    const float SPEED = 1;
    //[SerializeField] EnemyExample[] toStuneds = new EnemyExample[0];
    StunHandler stunHandler = new StunHandler();
    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.G))
    //    //    for (int i = 0; i < toStuneds.Length; i++)
    //    //        stunHandler.Stun(toStuneds[i]);

    //    if (timer < 1) timer = timer + SPEED * Time.deltaTime;
    //    else
    //    {
    //        timer = 0;
    //        stunHandler.FeedPulse();
    //    }
    //}

    /////////////////////////
    /// STUN HANDLER
    /////////////////////////
    public interface IStuned { void SetSeconds(int seconds); int GetSeconds(); void Ready(); }
    public class StunHandler
    {
        const int STUNNED_QUANTITY = 3, MIN_RANDOM = 10, MAX_RANDOM = 20;
        HashSet<IStuned> OnStuned = new HashSet<IStuned>();

        public void FeedPulse()// aca viene el pulso de 1 seg
        {
            if (OnStuned.Count > 0) OnStuned = UpdateStuns(OnStuned);
        }
        public void Stun(IStuned toStun, int secondsToStun = -1)
        {
            if (OnStuned.Count >= STUNNED_QUANTITY) return;
            OnStuned.Add(toStun); //hashet ignora el Add si ya lo tengo
            toStun.SetSeconds(secondsToStun == -1 ? Random.Range(MIN_RANDOM, MAX_RANDOM) : secondsToStun);
        }

        // Le puse para que retorne un HashSet para que actualice la coleccion en caliente sobre si misma
        // sacrifico un poco mas de memoria para reducir un poco la complejidad algorítmica
        // podria haber quitado el parametro de entrada y el de retorno y modificar directamente OnStuned
        // pero se me hace que iba a haber problemas con los SideEffects
        HashSet<IStuned> UpdateStuns(IEnumerable<IStuned> stunnedPeople)
        {
            HashSet<IStuned> hotData = new HashSet<IStuned>(stunnedPeople); //me hago una copia de la collecion original
            HashSet<IStuned> toRemove = new HashSet<IStuned>(); // creo un recipiente para guardar los que borro
            foreach (var s in hotData)
            {
                //obtengo la informacion de entrada
                var sec = s.GetSeconds();
                sec--;

                //proceso la informacion
                if (sec <= 0)
                {
                    s.SetSeconds(0);
                    s.Ready();
                    toRemove.Add(s);
                }
                else s.SetSeconds(sec);
            }
            hotData.ExceptWith(toRemove); //le hago la operacion de extraccion de los que ya estan ready
            return hotData; //se lo retorno a la coleccion original
        }
    }
}