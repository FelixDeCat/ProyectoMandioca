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
    float currentSpeed;
    float posLerp;
    float timer;

    public void StartCinematic()
    {
        transform.position = Main.instance.GetMyCamera().transform.position;
        transform.rotation = Main.instance.GetMyCamera().transform.rotation;
        Main.instance.GetMyCamera().GetComponent<Camera>().enabled = false;
        GetComponent<Camera>().enabled = true;
        moving = true;
        currentRotation = transform.rotation;
        currentCamPos = transform.position;
        currentSpeed = startSpeed;
    }

    void EndCinematic()
    {
        Debug.Log("Se terminó lo que se daba");
    }


    private void Update()
    {
        if (moving)
        {
            posLerp += Time.deltaTime * currentSpeed;
            transform.position = Vector3.Lerp(currentCamPos, waypoints[nextIndex].transform.position, posLerp);
            transform.rotation = Quaternion.Lerp(currentRotation, waypoints[nextIndex].transform.rotation, posLerp);

            if(posLerp >= 1)
            {
                current = waypoints[nextIndex];
                nextIndex += 1;
                currentCamPos = current.transform.position;
                currentRotation = current.transform.rotation;
                currentSpeed = current.changeSpeed;
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
            transform.position += UnityEngine.Random.insideUnitSphere * shakeAmmount;
        }
    }
}
