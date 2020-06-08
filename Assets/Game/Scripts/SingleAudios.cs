using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAudios : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    [SerializeField] string _nameOfClip;
    [SerializeField] Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.GetSoundPool(_nameOfClip, AudioGroups.GAME_FX, _clip);
        AudioManager.instance.PlaySound(_nameOfClip, myTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
