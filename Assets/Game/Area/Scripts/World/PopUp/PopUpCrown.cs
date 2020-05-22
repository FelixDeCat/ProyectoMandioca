using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCrown : MonoBehaviour
{
    public Transform _MyVIP;
    MeshRenderer _MyMesh;
    // Start is called before the first frame update
    void Start()
    {
        _MyMesh = GetComponent<MeshRenderer>();
        Main.instance.SetCrown(this);
        //_MyMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_MyVIP != null)
            transform.position = _MyVIP.position;
    }

    public void GetMyVIP(Transform VIP)
    {
        _MyVIP = VIP;
        transform.localScale = VIP.localScale;
        _MyMesh.enabled = true;
    }
    public void RemoveVIP(Transform VIP)
    {
        if (_MyVIP == VIP)
        {
            _MyVIP = null;
            _MyMesh.enabled = false;
        }
    }
}
