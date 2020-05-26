using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berseker : MonoBehaviour
{
    public Material _ppmat;
    CharacterHead myChar;

    private void Start()
    {
        if (myChar == null) myChar = Main.instance.GetChar();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _ppmat);
    }

    private void Update()
    {
        if (myChar.isBuffed == true)
        {
            _ppmat.SetFloat("_Berseker", 1);
            Debug.Log("IMbuf");
        }
        else
            _ppmat.SetFloat("_Berseker", 0f);
    }
}
