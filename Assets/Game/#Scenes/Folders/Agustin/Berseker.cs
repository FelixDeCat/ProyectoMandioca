using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berseker : MonoBehaviour
{
    public Material enterBerserk;
    public Material stayBerserk;
    public Material wings;
    CharacterHead myChar;
    public float fadeOutTime;
    float time;
    float frames = 10;
    bool oneTime = true;

    private void Start()
    {
        if (myChar == null) myChar = Main.instance.GetChar();
        frames = 10;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, enterBerserk);
        stayBerserk.SetFloat("_Value", 0);
        enterBerserk.SetFloat("_Value", 0);
        wings.SetFloat("_TransparencyValue", 0);
    }

    private void Update()
    {
        if (myChar.isBuffed == true)
        {
            if (oneTime) StartCoroutine(fadeShaderOut());
            stayBerserk.SetFloat("_Value", 1);
        }
        else
        {
            if (!oneTime) oneTime = true;
            stayBerserk.SetFloat("_Value", 0);
        }
    }

    IEnumerator fadeShaderOut()
    {
        oneTime = false;
        enterBerserk.SetFloat("_Value", 1);

        time = fadeOutTime / frames;
        float current = 1;

        for (int i = 0; i < frames; i++)
        {
            current -= 1 / frames;
            enterBerserk.SetFloat("_Value", current);

            wings.SetFloat("_TransparencyValue", current);

            yield return new WaitForSeconds(time);
        }
    }
}
