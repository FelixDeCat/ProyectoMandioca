using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteDeadCinematic : MonoBehaviour
{
    [SerializeField] float timeToDissappear = 2;
    [SerializeField] float dissappearTime = 3;
    [SerializeField] float waitTime = 2;
    [SerializeField] Renderer render = null;
    private const string varName = "_Opacity";

    [SerializeField] CameraCinematic cameraCine = null;

    public void StartCinematic()
    {
        Fades_Screens.instance.FadeOn(CinematicOn);
    }

    void CinematicOn()
    {
        StartCoroutine(WaitToCamera());
        cameraCine.StartCinematic(() => Fades_Screens.instance.FadeOff(() => { }));
    }

    void FadeOver()
    {
        StartCoroutine(Cinematic());
    }
    IEnumerator WaitToCamera()
    {
        yield return new WaitForSeconds(0.15f);
        Fades_Screens.instance.FadeOff(FadeOver);
    }

    IEnumerator Cinematic()
    {
        yield return new WaitForSeconds(timeToDissappear);

        float timer = dissappearTime;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            render.material.SetFloat(varName, Mathf.Lerp(0, 1, timer / dissappearTime));
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(waitTime);
        CinematicOff();
    }

    void CinematicOff()
    {
        Fades_Screens.instance.FadeOn(()=> { });
    }
}
