using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperativeCollection<T> where T : AsyncOperation
{
    T[] coll = new T[0];

    public void AddItem(T obj, int level_priority)
    {

    }

    void AddValue(T val)
    {
        int rezval = coll.Length + 1;
        T[] newcol = new T[rezval];
        for (int i = 0; i < rezval; i++)
        {
            if (i < rezval - 1) newcol[i] = coll[i];
            else newcol[i] = val;
        }
    }
}


#region ej: fran
//void exampel()
//{
//    float failsafeTime = Time.realtimeSinceStartup + tiempo_maximo_de_espera;
//    while ((conjunto_de_escenas_a_cargar.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }
//}
//void MeMori()
//{
//    Mando_a_todos_los_enemigos_a_pausa();
//    Pongo_Algun_Filtro_o_Feedback_Visual_De_Que_Me_Mori();
//    Mi_Animacion_Dramatica_De_Cai_Al_Suelo_O_Se_Abre_El_Suelo_(Fade_On);
//}
//void Fade_On() => Fade(true, Carga_Asincronica_De_la_Escena_De_Caronte); 
//void Carga_Asincronica_De_la_Escena_De_Caronte() => StartCoroutine(LoadScene("Caronte"));
//IEnumerator LoadScene(string scene)
//{
//    yield return Load(scene);
//    float failsafeTime = Time.realtimeSinceStartup + tiempo_maximo_de_espera;
//    while ((conjunto_de_escenas_a_cargar.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }
//    Termino_La_Carga();
//}

//void Termino_La_Carga() 
//{
//    Mi_Inicializacion_Combate_Caronte();
//    Inicializo_IA_Caronte();
//    Fade_Off(Animacion_de_Caigo_hacia_la_escena_o_aparezco_de_alguna_manera);
//}
//void Animacion_de_Caigo_hacia_la_escena_o_aparezco_de_alguna_manera() 
//{
//    Anim_Fall_Scene(Inicia_Combate_Caronte);
//}
//void Inicia_Combate_Caronte()
//{ 
//    // etc etc etc
//}


//private void Mando_a_todos_los_enemigos_a_pausa() { }
//void Pongo_Algun_Filtro_o_Feedback_Visual_De_Que_Me_Mori() { }
//void Mi_Animacion_Dramatica_De_Cai_Al_Suelo_O_Se_Abre_El_Suelo_(Action act) { }

//void Fade(bool val, Action end) { };

//void Mi_Inicializacion_Combate_Caronte() { }
//void Inicializo_IA_Caronte() { }
//void Fade_Off(Action act) { }

//void Anim_Fall_Scene(Action act) { }
#endregion
