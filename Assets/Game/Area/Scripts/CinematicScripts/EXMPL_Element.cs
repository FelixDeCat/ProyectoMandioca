using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXMPL_Element : MonoBehaviour, ICineElementable
{
    public Light light_example;
    NewPingPongLerp pingPong;
    public ParticleSystem particles;
    public TextMesh text;

    float timer;

    void Awake()
    {
        pingPong = new NewPingPongLerp(AnimLerpPingPong);
    }

    void AnimLerpPingPong(float cursor)
    {
        light_example.intensity = Mathf.Lerp(1,3, cursor);
        text.color = Color.Lerp(Color.red, Color.green, cursor);
    }

    public void OnBeginCinematic()
    {
        Debug.Log("Inicia elemento de cinematica");
        pingPong.Play(2);
        particles.Play();
        timer = 5;
    }

    public void OnEndCinematic()
    {
        Debug.Log("Termina elemento de cinematica");
        pingPong.Stop();
        particles.Stop();
        light_example.intensity = 0;
        text.text = "";
    }

    public void OnUpdateCinematic()
    {
        Debug.Log("Se updatea elemento cinematica");
        pingPong.Update();

        timer = timer - 1 * Time.deltaTime;
        text.text = "CINE_EVENT_ENDS: " + timer.ToString();

        if (timer <= 0)
        {
            timer = 5;
            CinematicController.Stop("ejemplo");
        }
    }
}
