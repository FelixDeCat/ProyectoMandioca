using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public List<BezierPoint> cameraPos;
    public Transform VerticalPos;
    public CharacterHead myPlayer;
    public Transform look;
    
    public float sliderTime = 1;

    void Start()
    {
    }

    void Update()
    {
        VerticalPos.transform.position = GetPointOnBezierCurve(cameraPos[0], cameraPos[1]);
    }

    public void ChangeView()
    {
        if (sliderTime >= 1)
        {
            sliderTime = 0;
            StartCoroutine(MoveSlider());
        }
    }

    Vector3 GetPointOnBezierCurve(BezierPoint ini, BezierPoint final)
    {
        Vector3 a = Vector3.Lerp(ini.transform.position, ini.son.transform.position, sliderTime);
        Vector3 b = Vector3.Lerp(ini.son.transform.position, final.son.transform.position, sliderTime);
        Vector3 c = Vector3.Lerp(final.son.transform.position, final.transform.position, sliderTime);

        Vector3 d = Vector3.Lerp(a, b, sliderTime);
        Vector3 e = Vector3.Lerp(b, c, sliderTime);
        Vector3 pointOnCurve = Vector3.Lerp(d, e, sliderTime);

        return pointOnCurve;
    }

    IEnumerator MoveSlider()
    {
        cameraPos.Reverse();

        while (sliderTime < 1)
        {
            sliderTime += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
