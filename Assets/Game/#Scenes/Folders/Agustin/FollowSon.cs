using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowSon : MonoBehaviour
{
    public GameObject sonToFollow;
    LineRenderer myRenderer;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.SetPosition(myRenderer.positionCount - 1, sonToFollow.transform.localPosition);
    }
}
