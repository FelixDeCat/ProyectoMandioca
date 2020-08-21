using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    public float timeToReturn;

    private void Start()
    {
        StartCoroutine(ToPool());
    }

    IEnumerator ToPool()
    {
        yield return new WaitForSeconds(timeToReturn);

        GetComponent<PlayObject>().ReturnToSpawner();
    }
}
