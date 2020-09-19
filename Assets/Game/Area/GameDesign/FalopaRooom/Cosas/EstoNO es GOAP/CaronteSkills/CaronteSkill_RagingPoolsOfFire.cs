using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteSkill_RagingPoolsOfFire : GOAP_Skills_Base
{
    [SerializeField] CustomSpawner fireSpawner;

    protected override void OnExecute()
    {
        fireSpawner.ActivateSpawner();
        StartCoroutine(FinishSkillIn());
    }

    IEnumerator FinishSkillIn()
    {
        yield return new WaitForSeconds(6);
        OnFinishSkill?.Invoke();
    }






    protected override void OnFixedUpdate()
    {

    }

    protected override void OnInitialize()
    {
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
}
