using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TextAnimCharXChar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtmesh;
    float timeSpacing = 0.1f;
    char[] completeText = new char[0];
    Action EndAnimationCallback = delegate { };

    public bool inAnimation;

    public void BeginAnim(string _txt, Action _OnEnd, float _timeSpacing = 0.07f)
    {
        txtmesh.text = "";
        timeSpacing = _timeSpacing;
        completeText = _txt.ToCharArray();
        StartCoroutine(Animate());
        EndAnimationCallback = _OnEnd;
        inAnimation = true;
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
