using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTestCharacter : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetButton("Horizontal") ) {
            rb.MovePosition(transform.position + transform.right * Input.GetAxis("Horizontal") * speed);
        }
    }
}
