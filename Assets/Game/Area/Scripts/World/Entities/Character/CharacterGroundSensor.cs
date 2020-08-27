using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundSensor : MonoBehaviour
{
    [SerializeField] float widht = 1;
    [SerializeField] float lenght = 1;
    [SerializeField] float height = 1;
    [SerializeField] LayerMask floorMask = 1 << 21;

    [SerializeField] float gravityMultiplier = 9.81f;
    [SerializeField] float minAceleration = 1;
    [SerializeField] float maxAceleration = 20;

    bool isGrounded;
    float timer;

    public float VelY { get; private set; }
    public bool IsInGround { get => isGrounded; private set { } }
    bool on;

    public void TurnOn()
    {
        on = true;
    }

    public void TurnOff()
    {
        on = false;
        VelY = 0;
    }

    private void Update()
    {
        if (!on) return;

        IsGrounded();

        if (!isGrounded)
        {
            VelY = Mathf.Clamp(VelY - timer * gravityMultiplier, -maxAceleration, - minAceleration);
            timer += Time.deltaTime;

            Debug.Log(VelY);

            DebugCustom.Log("Gravity", "Gravity", "TRUE");
        }
        else
        {
            timer = 1;

            VelY = 0;
            DebugCustom.Log("Gravity", "Gravity", "false");
        }
    }

    public void IsGrounded()
    {
        isGrounded = false;

        if (Physics.Raycast(transform.position, Vector3.down, height, floorMask))
            isGrounded = true;
        else if(Physics.Raycast(transform.position + Vector3.right * widht, Vector3.down, height, floorMask))
            isGrounded = true;
        else if (Physics.Raycast(transform.position + Vector3.left * widht, Vector3.down, height, floorMask))
            isGrounded = true;
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght, Vector3.down, height, floorMask))
            isGrounded = true;
        else if (Physics.Raycast(transform.position + Vector3.back * lenght, Vector3.down, height, floorMask))
            isGrounded = true;
    }

    public void AddForce(float force)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * height);
        Gizmos.DrawLine(transform.position + Vector3.right * widht, transform.position + Vector3.right * widht + (transform.position + Vector3.down * height));
        Gizmos.DrawLine(transform.position + Vector3.left * widht, transform.position + Vector3.left * widht + (transform.position + Vector3.down * height));
        Gizmos.DrawLine(transform.position + Vector3.forward * lenght, transform.position + Vector3.forward * lenght + (transform.position + Vector3.down * height));
        Gizmos.DrawLine(transform.position + Vector3.back * lenght, transform.position + Vector3.back * lenght + (transform.position + Vector3.down * height));
    }

}
