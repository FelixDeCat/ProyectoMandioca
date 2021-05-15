using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ClassicDoor : DoorCore
{
    ////////////////////////////////////////////////////////////////////////
    /// MUY IMPORTANTE !!!
    /// El canto de la puerta tiene que estar 
    /// apuntando al forward del Pivot
    ////////////////////////////////////////////////////////////////////////

    [Header("Classic Door Options")]
    [SerializeField] Transform pivot;

    [SerializeField] Transform look_at_to_open;
    [SerializeField] Transform look_at_to_close;

    Vector3 Forward_Open
    {
        get
        {
            var dir_open = look_at_to_open.transform.position - pivot.transform.position;
            dir_open.y = 0;
            return dir_open.normalized;
        }
    }
    Vector3 Forward_Close
    {
        get
        {
            var dir_open = look_at_to_close.transform.position - pivot.transform.position;
            dir_open.y = 0;
            return dir_open.normalized;
        }
    }

    [Header("Gizmos")]
    [SerializeField] bool draw_forward = true;
    [SerializeField] bool draw_up = true;
    [SerializeField] bool draw_Open_And_Close = true;

    private void Start()
    {
        pivot.forward = Forward_Close;
    }

    #region Condicion, si no tiene ninguna en especial, devolver true
    protected override bool CustomCondition() { return true; }
    #endregion

    #region Animacion
    protected override void LerpValue(float lerp_value)
    {
        pivot.forward = Vector3.Lerp(Forward_Close, Forward_Open, lerp_value);
    }
    #endregion

    #region FEEDBACKS
    [Header("UNITY EVENTS FEEDBACKS")]
    [SerializeField] ClassicDoorFeedbacks feedback;
    protected override void Feedback_On_Begin_Open() { feedback.Feedback_On_Begin_Open.Invoke(); }
    protected override void Feedback_On_End_Open() { feedback.Feedback_On_End_Open.Invoke(); }
    protected override void Feedback_On_Begin_Close() { feedback.Feedback_On_Begin_Close.Invoke(); }
    protected override void Feedback_On_End_Close() { feedback.Feedback_On_End_Close.Invoke(); }
    protected override void Feedback_On_Execution_Failed() { feedback.Feedback_On_Execution_Failed.Invoke(); }
    #endregion

    int DIST = 20;
    private void OnDrawGizmos()
    {
        if (pivot)
        {
            for (int i = 0; i < 10; i++)
            {
                if (draw_forward)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(pivot.position + (Vector3.up * i / 2), pivot.forward * 10);
                }
                if (draw_Open_And_Close)
                {
                    if (look_at_to_open)
                    {
                        var dir_open = look_at_to_open.transform.position - pivot.transform.position;
                        dir_open.y = 0;
                        Gizmos.color = Color.white;
                        Gizmos.DrawRay(pivot.position + (Vector3.up * i / 2), dir_open.normalized * 10);
                    }
                    if (look_at_to_close)
                    {
                        var dir_close = look_at_to_close.transform.position - pivot.transform.position;
                        dir_close.y = 0;
                        Gizmos.color = Color.black;
                        Gizmos.DrawRay(pivot.position + (Vector3.up * i / 2), dir_close.normalized * 10);
                    }
                }
            }

            if (draw_up)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(pivot.position, pivot.up * DIST);
            }
        }
    }

    [System.Serializable]
    internal class ClassicDoorFeedbacks
    {
        [SerializeField] internal UnityEvent
        Feedback_On_Begin_Open,
        Feedback_On_End_Open,
        Feedback_On_Begin_Close,
        Feedback_On_End_Close,
        Feedback_On_Execution_Failed;
    }
}


