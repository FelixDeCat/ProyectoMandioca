using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TextAnimCharXChar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtmesh = null;
    float timeSpacing = 0.1f;
    char[] completeText = new char[0];
    Action EndAnimationCallback = delegate { };

    public bool inAnimation;

    //new 
    bool anim = false;
    float timer;

    public void BeginAnim(string _txt, Action _OnEnd, float _timeSpacing = 0.04f)
    {
        txtmesh.text = "";
        completeText = new char[0];

        timeSpacing = _timeSpacing;
        completeText = _txt.ToCharArray();

        StopAllCoroutines();
        StartCoroutine(Animate());
        EndAnimationCallback = _OnEnd;
        inAnimation = true;
    }

    private void Update()
    {
        DebugCustom.Log("Dialogue debug", "InAnim", inAnimation);
    }

    public void Force(string s)
    {
        txtmesh.text = s;
        
        inAnimation = false;
        StopAllCoroutines();
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < completeText.Length; i++)
        {
            yield return new WaitForSeconds(timeSpacing);
            txtmesh.text += completeText[i];
        }
        inAnimation = false;
        EndAnimationCallback.Invoke();
    }
}
