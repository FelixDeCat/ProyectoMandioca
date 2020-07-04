using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _EndPos;
    [SerializeField] float _speed;
    [SerializeField] bool _goBack;
    public bool active;
    [SerializeField] float _transitionTime;
    float _currentTimer;
    [SerializeField] TriggerOfPiston trigger;


    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos.position;
        trigger.getPiston(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (!_goBack)
        {
            transform.position = Vector3.Lerp(transform.position, _EndPos.position, Time.deltaTime* _speed);
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _transitionTime)
            {
                _goBack = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _startPos.position, Time.deltaTime* _speed);
            _currentTimer -= Time.deltaTime;
            if (_currentTimer <= 0)
            {
                _goBack = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            active = true;
        }
    }
}
