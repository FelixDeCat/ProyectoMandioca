using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPalanca : MonoBehaviour
{
    Animator myAnim;
    [SerializeField] GameObject[] objsTurnOff;

    bool isOn = false;
    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    public void Anim()
    {
        if (!isOn)
        {
            myAnim.Play("PalancaOn");
            for (int i = 0; i < objsTurnOff.Length; i++)
            {
                objsTurnOff[i].SetActive(false);
            }
        }
    }
    public void AnimOff()
    {
        myAnim.Play("PalancaOff");
        for (int i = 0; i < objsTurnOff.Length; i++)
        {
            objsTurnOff[i].SetActive(true);
        }
    }
}
