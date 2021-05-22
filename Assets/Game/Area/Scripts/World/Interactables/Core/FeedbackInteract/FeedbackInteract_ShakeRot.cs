using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_ShakeRot : FeedbackInteractBase
{
    [SerializeField] Transform torot = null;
    Vector3 startPos;
    [SerializeField] float shakeAmmount = 5;
    [SerializeField] float shakeTime = 0.2f;
    float timer;

    private void Start()
    {
        startPos = torot.localEulerAngles;
    }
    protected override void OnShow()
    {
        Debug.Log("que onda?");
        if (timer < shakeTime)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;

        Vector3 _randomPos = startPos + (Random.insideUnitSphere * shakeAmmount);
        torot.localEulerAngles = _randomPos;
    }
    protected override void OnHide() { if (torot) torot.localEulerAngles = startPos; timer = 0; }
    protected override void On_Condicional_Update()
    {
    }

    protected override void On_Permanent_Update()
    {
    }
}
