using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _EndPos;
    [SerializeField] float _speed;
    [SerializeField] bool _goBack;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (!_goBack)
        {
            transform.position = Vector3.Lerp(transform.position, _EndPos.position, Time.deltaTime * _speed);
            
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _startPos.position, Time.deltaTime * _speed);
           
        }
    }
    public void activate(bool activate)
    {
        active = activate;
        if (_goBack)
        {
            _goBack = false;
        }
        else
        {
            _goBack = true;
        }
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterHead>())
        {
            collision.gameObject.GetComponent<CharacterHead>().transform.parent = this.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterHead>())
        {
            collision.gameObject.GetComponent<CharacterHead>().transform.parent = null;
        }
    }
}
