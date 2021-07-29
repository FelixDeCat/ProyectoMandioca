using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelemacoMovetePorFavor : MonoBehaviour
{
    public Transform[] positions;
    public GameObject ObjectToMove;
    public float MoveSpeed = 8;
    Coroutine MoveIE;

    void Start()
    {
        StartCoroutine(moveObject());
    }

    IEnumerator moveObject()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            MoveIE = StartCoroutine(Moving(i));
            yield return MoveIE;
        }
    }

    IEnumerator Moving(int currentPosition)
    {
        while (ObjectToMove.transform != positions[currentPosition])
        {
            ObjectToMove.transform.position = Vector3.MoveTowards(ObjectToMove.transform.position, new Vector3(positions[currentPosition].position.x, positions[currentPosition].position.y, positions[currentPosition].position.z), MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
