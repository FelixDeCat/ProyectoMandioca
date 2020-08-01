using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenPillar : MonoBehaviour
{
    DamageReceiver dmg;
    [SerializeField] GameObject pillarFallen;
    [SerializeField] GameObject originPillar;
    _Base_Life_System _BLS;
    // Start is called before the first frame update
    void Start()
    {
        dmg = GetComponent<DamageReceiver>();
        _BLS = GetComponent<_Base_Life_System>();
        originPillar.SetActive(true);
        pillarFallen.SetActive(false);
        _BLS.Initialize();
        _BLS.CreateADummyLifeSystem();
        dmg.Initialize(
            transform,
            () => { return false; },
            (x) => { },
            (x) => { GetHit(); },
            GetComponent<Rigidbody>(),
            _BLS);
    }

    

    void GetHit()
    {
        Debug.Log("asdasdasdsad");
        originPillar.SetActive(false);
        pillarFallen.SetActive(true);
    }
}
