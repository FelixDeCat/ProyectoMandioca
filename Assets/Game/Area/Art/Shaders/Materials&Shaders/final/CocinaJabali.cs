using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CocinaJabali : MonoBehaviour
{
    public Renderer render;
    public float val_cook;
    public GameObject Jabali;
    public Transform rotator;

    public GameObject fire;

    public UnityEvent OnCooked;
    public UnityEvent OnBurned;

    public GameObject effect_temperature;

    bool isCooked;

    float timercook;
    bool anim_cook;
    public float cook_speed;

    bool inCD;
    float timer_CD;

    private void Start()
    {
        fire.SetActive(false);
    }

    public void OnEnterInteract() { /*render.material.SetFloat("_ASEOutlineWidth", 0.01f);*/ }
    public void OnExitInteract() { /*render.material.SetFloat("_ASEOutlineWidth", 0);*/ }

    public void Press()
    {
        if (isCooked)
        {
            if (!inCD)
            {
                inCD = true;
                Jabali.SetActive(false);
                Invoke("OnReset", 5f);
            }
        }
        else
        {
            anim_cook = true;
            fire.SetActive(true);
            effect_temperature.SetActive(true);
        }
    }
    public void Release()
    {
        fire.SetActive(false);
        anim_cook = false;
        effect_temperature.SetActive(false);
    }

    public void OnReset()
    {
        inCD = false;
        Jabali.SetActive(true);
        isCooked = false;
        cookedOneShoot = false;
        anim_cook = false;
        timercook = 0;
        render.material.SetFloat("_CookHandler", 0);
        render.material.SetColor("_ASEOutlineColor", Color.white);
    }

    bool cookedOneShoot;

    void Update()
    {
        Debug.Log("Update");
        if (anim_cook)
        {
            if (timercook < 2)
            {
                timercook = timercook + cook_speed * Time.deltaTime;
                render.material.SetFloat("_CookHandler", timercook);

                rotator.Rotate(0, 0, 1.5f);

                if (timercook >= 1)
                {
                    if (!cookedOneShoot)
                    {
                        cookedOneShoot = true;
                        isCooked = true;
                        OnCooked.Invoke();
                        render.material.SetColor("_ASEOutlineColor", Color.green);
                    }
                }
            }
            else
            {
                OnBurned.Invoke();
                timercook = 0;
                anim_cook = false;
                render.material.SetColor("_ASEOutlineColor", Color.red);
            }
        }
    }
}
