using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrutineTest : MonoBehaviour
{

    Coroutine cor;

    public int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Empiezo");
            cor = StartCoroutine(ienum());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Freno");
            StopCoroutine(cor);
        }
    }
    
    IEnumerator ienum()
    {
        count = 0;
        while (count < 10)
        {
            count++;
            Debug.Log("Sumo");
            yield return new WaitForSeconds(1);
            Debug.Log("Intermedio");
            yield return new WaitForSeconds(2);
            Debug.Log("Final");
        }
        
    }
}
