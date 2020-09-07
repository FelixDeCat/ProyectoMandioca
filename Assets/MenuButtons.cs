using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Animator animLoadM;
    public Animator animSttngsM;
    public Animator animCredits;

    public Animator fadeAnim;

    public Button[] mainButtons;

    private Animator _currentAnim;
  
    public void StarButton()
    {
        //rellenar
    }
    public void LoadButton() //aca abro un mini menu con animator ,por el momento podrian acceder a las demas escenas desde aca
    {
        animLoadM.SetTrigger("Open");
        foreach (var item in mainButtons)
        {
            item.interactable = false;
        }
    }

    public void Settings()
    {
        //que lo codee su vieja
    }
    public void Credits()
    {
        animCredits.SetTrigger("Open");
        _currentAnim = animCredits;
        foreach (var item in mainButtons)
        {
            item.interactable = false;
        }
        fadeAnim.SetTrigger("MenuFade");
    }

    public void ReactivateButtons(Animator currentAnim)//cuando quiera salir de los miniMenues
    {
        fadeAnim.SetTrigger("Off");
        currentAnim.SetTrigger("Off");
        foreach(var item in mainButtons)
        {
            item.interactable = true;
        }
    }
    private void Update()
    {
        if (_currentAnim != null&&Input.anyKeyDown)
        {
            ReactivateButtons(_currentAnim);
        }
    }
}
