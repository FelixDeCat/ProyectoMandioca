using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GodRoom : MonoBehaviour
{
    InteractableTeleport teleport;

    public Transform transform_destino;

    public UnityEvent EnterGodRoom;
    public UnityEvent ExitGodRoom;
    private void Awake()
    {
        //teleport = GetComponent<InteractableTeleport>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R))
            {
                Main.instance.GetChar().GetCharMove().StopDamageFall();
                Main.instance.GetChar().transform.position = transform_destino.position;
                OnEnterGodRoom();
            }

        }
    }

    public void UE_OnExitGodRoom()
    {
        ExitGodRoom.Invoke();
    }
    public void OnEnterGodRoom()
    {
        EnterGodRoom.Invoke();
    }
}
