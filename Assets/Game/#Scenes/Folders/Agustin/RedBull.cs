using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBull : MonoBehaviour
{
    public Shader shader;
    Material _ppmat;
    CharacterHead myChar;
    public Vector2 speed;

    private void Start()
    {
        _ppmat = new Material(shader);
        if (myChar == null) myChar = Main.instance.GetChar();
        _ppmat.SetVector("_Speed", speed);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _ppmat);
    }

    private void Update()
    {
        if(myChar.isBuffed == true)
        {
            _ppmat.SetFloat("_Energy", 0.04f);
            Debug.Log("BufRedBul");
        }
        else
            _ppmat.SetFloat("_Energy", 0f);
    }
}
