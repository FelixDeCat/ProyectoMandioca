using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Anim_Code : UI_AnimBase
{
    public bool test_stay_in_my_place;

    CanvasGroup myCanvasGroup;

    [Range(1, 20)]
    public float speed = 9;

    float timer = 0;
    const float time_to_go = 1;
    bool anim;
    bool go;

    Vector3 currentpos;
    Vector3 initHidepos;
    Vector3 finalHidepos;

    public bool usePosition;

    public enum AppearSide { Up, Down, Left, Right }
    public AppearSide appearSide;
    public AppearSide dissapearSide;

    private void Start()
    {
        myCanvasGroup = GetComponentInChildren<CanvasGroup>();
        if (myCanvasGroup) myCanvasGroup.alpha = 0;

        if (usePosition)
        {
            currentpos = transform.localPosition;
            switch (appearSide)
            {
                case AppearSide.Up: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 500, transform.localPosition.z); break;
                case AppearSide.Down: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 500, transform.localPosition.z); break;
                case AppearSide.Left: initHidepos = new Vector3(transform.localPosition.x - 800, transform.localPosition.y, transform.localPosition.z); break;
                case AppearSide.Right: initHidepos = new Vector3(transform.localPosition.x + 800, transform.localPosition.y, transform.localPosition.z); break;
            }

            switch (dissapearSide)
            {
                case AppearSide.Up: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 500, transform.localPosition.z); break;
                case AppearSide.Down: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 500, transform.localPosition.z); break;
                case AppearSide.Left: finalHidepos = new Vector3(transform.localPosition.x - 800, transform.localPosition.y, transform.localPosition.z); break;
                case AppearSide.Right: finalHidepos = new Vector3(transform.localPosition.x + 800, transform.localPosition.y, transform.localPosition.z); break;
            }
            if (!test_stay_in_my_place) transform.localPosition = initHidepos;
            else transform.localPosition = currentpos;
        }
        else
        {
            
        }
    }

    public void ChangeAppearAndDisappearSide(AppearSide appear, AppearSide disappear)
    {
        appearSide = appear;
        dissapearSide = disappear;

        switch (appearSide)
        {
            case AppearSide.Up: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 500, transform.localPosition.z); break;
            case AppearSide.Down: initHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 500, transform.localPosition.z); break;
            case AppearSide.Left: initHidepos = new Vector3(transform.localPosition.x - 800, transform.localPosition.y, transform.localPosition.z); break;
            case AppearSide.Right: initHidepos = new Vector3(transform.localPosition.x + 800, transform.localPosition.y, transform.localPosition.z); break;
        }

        switch (dissapearSide)
        {
            case AppearSide.Up: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 500, transform.localPosition.z); break;
            case AppearSide.Down: finalHidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 500, transform.localPosition.z); break;
            case AppearSide.Left: finalHidepos = new Vector3(transform.localPosition.x - 800, transform.localPosition.y, transform.localPosition.z); break;
            case AppearSide.Right: finalHidepos = new Vector3(transform.localPosition.x + 800, transform.localPosition.y, transform.localPosition.z); break;
        }
    }

    protected override void OnOpen()
    {
        anim = true;
        go = true;
    }

    protected override void OnClose()
    {
        anim = true;
        go = false;
    }

    public void OnGo(float time_value) 
    {
        if (usePosition) transform.localPosition = Vector3.Lerp(initHidepos, currentpos, time_value);
        if(myCanvasGroup) myCanvasGroup.alpha = time_value;
    }
    public void OnBack(float time_value) 
    {
        if (usePosition) transform.localPosition = Vector3.Lerp(currentpos, finalHidepos, time_value);
        if (myCanvasGroup) myCanvasGroup.alpha = Mathf.Lerp(1,0,time_value);
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_go)
            {
                timer = timer + speed * Time.deltaTime;

                if (go)
                {
                    OnGo(timer);
                }
                else
                {
                    OnBack(timer);
                }
            }
            else
            {
                timer = 0;
                anim = false;
                if (go) { ExecuteEndOpenAnimation(); }
                else { ExecuteEndCloseAnimation(); }
            }
        }
    }
}
