using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainCameraCinematic : MonoBehaviour
{
    [SerializeField] WaypointCamera[] waypoints = new WaypointCamera[0];
    [SerializeField] UnityEvent OnStartCinematic = new UnityEvent();
    [SerializeField] UnityEvent OnEndCinematic = new UnityEvent();
    [SerializeField] float startSpeed = 2;

    bool shaking;
    bool moving;
    float shakeAmmount;
    Vector3 currentCamPos;
    Quaternion currentRotation;
    int nextIndex = 0;
    WaypointCamera current;
    float currentTimeToMove;
    float posLerp;
    float timer;
    bool updating = false;

    public void StartCinematic()
    {
        transform.parent.position = Main.instance.GetMyCamera().transform.position;
        transform.parent.rotation = Main.instance.GetMyCamera().transform.rotation;
        Main.instance.GetMyCamera().GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;
        moving = true;
        currentRotation = transform.parent.rotation;
        currentCamPos = transform.parent.position;
        currentTimeToMove = startSpeed;
        updating = true;
        Main.instance.GetChar().Pause();
    }

    void EndCinematic()
    {
        updating = false;
        Fades_Screens.instance.FadeOn(() => { 
            Fades_Screens.instance.FadeOff(()=> { });
            Main.instance.GetChar().Resume(); 
            OnEndCinematic.Invoke();
            Main.instance.GetMyCamera().GetComponent<Camera>().enabled = true;
            GetComponent<Camera>().enabled = false;
        });
        Debug.Log("Se terminó lo que se daba");
    }


    private void Update()
    {
        if (!updating) return;

        if (moving)
        {
            posLerp += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(currentCamPos, waypoints[nextIndex].transform.position, posLerp / currentTimeToMove);
            transform.parent.rotation = Quaternion.Lerp(currentRotation, waypoints[nextIndex].transform.rotation, posLerp / currentTimeToMove);

            if(posLerp / currentTimeToMove >= 1)
            {
                current = waypoints[nextIndex];
                nextIndex += 1;
                currentCamPos = current.transform.position;
                currentRotation = current.transform.rotation;
                currentTimeToMove = current.changeSpeed;
                current.EnterWaypoint();
                posLerp = 0;
                moving = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= current.durationInWaypoint)
            {
                if(nextIndex >= waypoints.Length)
                {
                    EndCinematic();
                    return;
                }
                moving = true;
                timer = 0;
                current.ExitWaypoint();
            }
        }
    }

    private void LateUpdate()
    {
        if (shaking)
        {
            transform.position = transform.parent.position + UnityEngine.Random.insideUnitSphere * shakeAmmount;
        }
    }

    public void BeginShake(float _shakeAmmount)
    {
        shaking = true;
        shakeAmmount = _shakeAmmount;
    }

    public void EndShake()
    {
        transform.position = transform.parent.position;
        shaking = false;
    }
}
