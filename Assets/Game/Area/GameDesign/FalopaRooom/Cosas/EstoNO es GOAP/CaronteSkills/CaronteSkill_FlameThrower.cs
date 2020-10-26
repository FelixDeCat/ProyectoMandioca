using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class CaronteSkill_FlameThrower : GOAP_Skills_Base
{
    [SerializeField] ParticleSystem tornadoFire = null;
    [SerializeField] float duration = 2;
    [SerializeField] GameObject myTrig = null;

    DamageData dmgData;

    protected override void OnEndSkill()
    {
        tornadoFire.Stop();
        myTrig.SetActive(false);
        owner.GetComponent<Ente>().OnSkillAction -= ActivateFlames;
        owner.GetComponentInChildren<Animator>().SetTrigger("finishSkill");
        OnFinishSkill?.Invoke();
    }

    protected override void OnExecute()
    {
        owner.GetComponentInChildren<Animator>().SetTrigger("flameThrower");
        owner.GetComponent<Ente>().OnSkillAction += ActivateFlames;
    }

    void ActivateFlames()
    {
        tornadoFire.Play();
        StartCoroutine(FlameThrower());
    }

    IEnumerator FlameThrower()
    {
        float count = 0;
        myTrig.SetActive(true);
        while(count < duration)
        {
            owner.LookAt(heroRoot);
            count += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        EndSkill();
    }

    public void MakeDamage()
    {
        Main.instance.GetChar().DamageReceiver().TakeDamage(dmgData);
    }

    protected override void OnFixedUpdate()
    {
        
    }

    protected override void OnInitialize()
    {
        dmgData = GetComponent<DamageData>().SetDamage(500).SetDamageType(Damagetype.Normal);
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnResume()
    {
        
    }

    protected override void OnTurnOff()
    {
        
    }

    protected override void OnTurnOn()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnInterruptSkill()
    {
        throw new System.NotImplementedException();
    }
}
