using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berseker : MonoBehaviour
{
    public Material _ppmat;
    public Material _ppmat2;
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
            _ppmat.SetFloat("_Value", 1);
            _ppmat2.SetFloat("_Value", 1);
        }
        else
        {
            _ppmat.SetFloat("_Value", 0);
            _ppmat2.SetFloat("_Value", 0);
        }
    }
}
