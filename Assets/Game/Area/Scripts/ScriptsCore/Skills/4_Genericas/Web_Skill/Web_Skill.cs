using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web_Skill : SkillBase
{
    
    [Header("Skill Settings")]
    [SerializeField] private Web _web_pf = null;
    [SerializeField] private float _webLifeTime = 8;
    [Range(0,1)]
    [SerializeField] private float _percentSlowed = 0.5f;
    
    
    [SerializeField] private int _totalWebsSimultaneas = 5;
    [SerializeField] private bool deleteFirstWeb = true;
    

    private Queue<Web> _websDeployed;
    private CharacterHead _hero;
    
    protected override void OnBeginSkill()
    {
        
        
        //Creo la queue de redes
        if(_websDeployed == null)
            _websDeployed = new Queue<Web>();
        //Busco al hero
        if(_hero == null)
            _hero = FindObjectOfType<CharacterHead>();

       _hero.AddListenerToDash(WebFabrication);
    }

    protected override void OnEndSkill()
    {
        Debug.Log("termina el skill");
        _hero.RemoveListenerToDash(WebFabrication);
    }

    protected override void OnUpdateSkill()
    {
        
    }

    private void WebFabrication()
    {
        //Si todavia puedo tirar mas redes, las tiro
        if (_websDeployed.Count < _totalWebsSimultaneas)
        {
            CreateNewWeb();
            return;
        }

        //Si tengo mi maximo de redes tiradas y "deleteFirstWeb" esta en true, voy a borrar la primer red 
        //que tire para poder tirar otra nueva
        if (_websDeployed.Count >= _totalWebsSimultaneas && deleteFirstWeb)
        {
            DeleteFirstWeb();
            CreateNewWeb();
        }
    }

    private void CreateNewWeb()
    {
        Web newWeb = Instantiate(_web_pf, _hero.transform.position, Quaternion.identity);
        newWeb.ConfigureWeb(_webLifeTime, _percentSlowed, () => _websDeployed.Dequeue());
        _websDeployed.Enqueue(newWeb);
    }

    private void DeleteFirstWeb()
    {
        Web dqWeb = _websDeployed.Dequeue();
        Destroy(dqWeb.gameObject);
    }
}
