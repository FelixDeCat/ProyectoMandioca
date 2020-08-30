using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundSensor : MonoBehaviour
{
    public Action<float> GroundOneShot;

    [SerializeField] float widht = 0.2f;
    [SerializeField] float lenght = 0.2f;
    [SerializeField] float height = 0.3f;
    [SerializeField] LayerMask floorMask = 1 << 21;

    [SerializeField] float gravityMultiplier = 0.2f;
    [SerializeField] float maxAceleration = 15;

    bool isGrounded;
    float timer;

    public float VelY { get; private set; }
    public bool IsInGround { get => isGrounded; private set { } }
    bool on;

    private void Start()
    {
        GroundOneShot += InGroundOneShot;
    }

    public void TurnOn()
    {
        on = true;
        StartCoroutine(OnUpdate());
    }

    public void TurnOff()
    {
        on = false;
        VelY = 0;
    }


    IEnumerator OnUpdate()
    {
        while (on)
        {
            IsGrounded();

            if (!isGrounded)
            {
                VelY = Mathf.Clamp(VelY - timer * gravityMultiplier, -maxAceleration, 15);
                timer += Time.deltaTime;

                DebugCustom.Log("Gravity", "Gravity", "TRUE");
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void IsGrounded()
    {
        RaycastHit hit;

        if (GroundOneShot == null) { Debug.LogWarning("perdon, no se quien esta con esto, pero le puse esto rapido para que no tire errores para zafar por ahora, firma. Felito el más piola XD. pdta: cuando las mandragoras salen tira null referece el GroundOneShot"); return; }

        if (Physics.Raycast(transform.position, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if(Physics.Raycast(transform.position + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.transform.position.y);
        else
            isGrounded = false;
    }

    void InGroundOneShot(float y)
    {
        if (!isGrounded)
        {
            timer = 1;

            VelY = 0;
            DebugCustom.Log("Gravity", "Gravity", "false");
            isGrounded = true;
        }
    }

    public void AddForce(float force)
    {
        VelY += force;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * height);
        Gizmos.DrawLine(transform.position + transform.right * widht, transform.position + transform.right * widht + ( -transform.up * height));
        Gizmos.DrawLine(transform.position - transform.right * widht, transform.position - transform.right * widht + ( -transform.up * height));
        Gizmos.DrawLine(transform.position + transform.forward * lenght, transform.position + transform.forward * lenght + (-transform.up * height));
        Gizmos.DrawLine(transform.position - transform.forward * lenght, transform.position - transform.forward * lenght + ( -transform.up * height));
    }

}
