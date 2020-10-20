using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenShower : MonoBehaviour
{
    [SerializeField] float height = 6;
    [SerializeField] float timeToRagdoll = 2;

    bool falling;
    Vector3 initPos;
    private void Awake()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        if(!falling && transform.position.y <= height)
        {
            falling = true;
            StartCoroutine(Reappear());
        }
    }

    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(timeToRagdoll);
        transform.position = initPos;
        falling = false;
    }
}
