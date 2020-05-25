using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//con esto nuevo de tener las skills en 3D no olvidar tildar el bool por editor que especifica que es 3D
public class SkillActive_NewSkill : SkillActivas
{
    //esto es cuando la skill se equipa
    protected override void OnBeginSkill() { Debug.Log("Equipando nueva Skill"); }
    //esto es cuando la skill se desequipa
    protected override void OnEndSkill() { Debug.Log("Desequipando nueva Skill"); }
    //esto se ejecuta cada frame mientras tenga equipada la skill
    protected override void OnUpdateSkill() { Debug.Log("Updatenado nueva Skill"); }

    // [por editor] si el bool "one_time_use = true" ejecuto esto una sola vez
    // es sirve mas que nada si es algo oneshot
    // ej me curo tanta cantidad, stuneo a todos, ejecuto una animacion que hace algo, etc, etc
    protected override void OnOneShotExecute() { Debug.Log("Usando nueva Skill"); }

    // [por editor] si el bool "one_time_use = false" ejecuto esto como si fuera un estado
    // es sirve mas que nada si es algo que tiene entrada y salida o que queremos ejecutar durante el tiempo
    // ej lluvia de flechas, congelamiento durante el tiempo, spin and stun, rebota la luz con escudo cierto tiempo, etc, etc, etc
    protected override void OnStartUse() { Debug.Log("StartUse nueva Skill"); }
    protected override void OnStopUse() { Debug.Log("StopUse nueva Skill"); }
    protected override void OnUpdateUse() { Debug.Log("UpdateUse nueva Skill"); }

    protected override void CoroutineUpdate()
    {
        Debug.Log("custom update");
    }
}
