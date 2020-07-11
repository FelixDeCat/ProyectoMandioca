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
    [SerializeField] float _transitionTimeDown;
    [SerializeField] float _transitionTimeUp;
    float _currentTimer;
    [SerializeField] TriggerOfPiston trigger;


    
    // Start is called before the first frame update
    void Start()
    {
        if(!_goBack)
            transform.position = _startPos.position;
        else
            transform.position = _EndPos.position;


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
            if (_currentTimer >= _transitionTimeDown)
            {
                _currentTimer = 0;
                _goBack = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _startPos.position, Time.deltaTime* _speed);
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= _transitionTimeUp)
            {
                _currentTimer = 0;
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
