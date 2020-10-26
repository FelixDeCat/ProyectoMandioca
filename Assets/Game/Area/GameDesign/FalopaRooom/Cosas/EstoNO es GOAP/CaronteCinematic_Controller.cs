using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteCinematic_Controller : MonoBehaviour
{
    [SerializeField] List<Transform> nodes = new List<Transform>();
    [SerializeField] GameObject boat = null;
    [SerializeField] float speed = 7;
    [SerializeField] GameObject caronteAnimator = null;

    public event Action OnFinishCinematic;

    int _currentNode = 0;
    
    public void StartCinematic()
    {
        StartCoroutine(MoveBoat());
        caronteAnimator.GetComponent<AnimEvent>().Add_Callback("finishCinematic", FinishAnimation);
    }

    void FinishAnimation()
    {
        OnFinishCinematic?.Invoke();
        Destroy(caronteAnimator);
    }

    IEnumerator MoveBoat()
    {
        while(_currentNode < nodes.Count)
        {
            Vector3 dir = (nodes[_currentNode].position - boat.transform.position).normalized;

            boat.transform.forward = Vector3.Lerp(boat.transform.forward, dir, 0.3f * Time.fixedDeltaTime);

            if (Vector3.Distance(nodes[_currentNode].position, boat.transform.position) >= 5)
            {
                boat.transform.position += boat.transform.forward * speed * Time.fixedDeltaTime;
            }
            else
            {
                _currentNode++;
            }

            yield return new WaitForEndOfFrame();
        }

        caronteAnimator.GetComponent<Animator>().SetTrigger("downDeBoat");

        
    }
       
}
