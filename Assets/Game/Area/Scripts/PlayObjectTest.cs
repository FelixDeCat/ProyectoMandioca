using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayObjectTest : PlayObject
{
    public GameObject GO_ON;
    public GameObject GO_OFF;

    float timer;
    protected override void OnFixedUpdate() { }
    protected override void OnInitialize()
    {
        CreateCube(Color.magenta, this.transform.position);
    }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { GO_ON.SetActive(false); GO_OFF.SetActive(true); }
    protected override void OnTurnOn() { GO_ON.SetActive(true); GO_OFF.SetActive(false); }
    protected override void OnUpdate()
    {
        Debug.Log("EXECUTE");
        if (timer < 0.1f)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            CreateCube(Color.cyan, this.transform.position + this.transform.right);
            timer = 0;
        }
    }


    public void CreateCube(Color color, Vector3 pos)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = pos;
        go.GetComponent<Renderer>().material.SetColor("_Color", color);
        go.AddComponent<Rigidbody>();
    }
}
