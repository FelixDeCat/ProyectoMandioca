using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI3D_CursorActiva : MonoBehaviour
{
    public float transition_speed_multiply = 3f;
    float timer = 0;
    bool animate;

    Vector3 currentPos;
    Vector3 destinity;

    public FeedbackOneInteract_ScaleByCurve feedbackClick;

    public void GoToPosition(Vector3 pos)
    {
        Debug.Log("Entro un vex");
        animate = true;
        timer = 0;
        currentPos = this.transform.position;
        destinity = pos;
    }

    public void ExecuteUse()
    {
        feedbackClick.Execute();
    }

    private void Update()
    {
        if (animate)
        {
            if (timer < 1)
            {
                timer = timer + transition_speed_multiply * Time.deltaTime;
                transform.position = Vector3.Lerp(currentPos, destinity, timer);
            }
            else
            {
                animate = false;
                timer = 0;
            }
        }
    }


    #region Fades

    public void FadeOut(float fadeSpeed)
    {
        var images = GetComponentsInChildren<Image>();
        
        StartCoroutine(StartFadeOut(images ,fadeSpeed));

    }

    public void FadeIn(float fadeSpeed)
    {
        var images = GetComponentsInChildren<Image>();
        
        StartCoroutine(StartFadeIn(images ,fadeSpeed));
    }
    
    IEnumerator StartFadeOut(Image[] images,float fadeSpeed)
    {
        foreach (var img in images)
        {
            while (img.color.a > 0)
            {
                Color newColor = img.color;
                newColor.a -= Time.deltaTime * fadeSpeed;
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    IEnumerator StartFadeIn(Image[] images,float fadeSpeed)
    {
        foreach (var img in images)
        {
            while (img.color.a < 1)
            {
                Color newColor = img.color;
                newColor.a += Time.deltaTime * fadeSpeed;
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }

    #endregion
    

}
