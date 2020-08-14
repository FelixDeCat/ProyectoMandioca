using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Active : MonoBehaviour, ISubmitHandler
{
    public Image img;
    public Image bkg;

    public ParticleSystem particle;

    public void SetCooldown(float time)
    {
        img.fillAmount = time;
    }

    public void FeedbackEndCooldown()
    {
        particle.Play();
    }

    public void SetSprite(Sprite sp)
    {
        img.sprite = sp;
        bkg.sprite = sp;
    }

    public void SetColor(Color color)
    {
        //img.color = color; 
        //bkg.color = color;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        
    }
}
