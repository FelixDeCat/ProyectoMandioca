using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillInfo", menuName = "Skills/SkillInfo", order = 1)]
public class SkillInfo : ScriptableObject
{
    //los 3 tipos control, culpa, obligacion
    public SkillType skilltype;

    //ejemplo, furia de zeus
    public string skill_name = "default_name";

    //aca se puede poner, la parte explicativa relacionada al lore, de porque puedo tener esta habilidad,
    //por ejemplo, cada cierto tiempo zeus libera toda su furia y con tu espada divina podrás absorber y almacenar rayos divinos directos desde el cielo para liberarlos contra tus enemigos
    [Multiline(15)] public string description_lore = "default_description_lore";

    //aca se puede poner, la parte explicativa mas técnica
    //por ejemplo, tienes una probabilidad del 7% de almacenar un rayo, este se liberara con el primer ataque y encadenará hasta 5 enemigos cercanos aplicandoles [electrocucion] quitándoles 10 de vida cada 1 segundo, 
    //mientras estos tengan el efecto activo, podrán activar mecanismos divinos o electrificar objetos metálicos cercanos
    [Multiline(15)] public string description_technical = "default_description_technical";
    
    public Sprite img_blocked;
    public Sprite img_avaliable;
    public Sprite img_actived;
    public GameObject model;
}