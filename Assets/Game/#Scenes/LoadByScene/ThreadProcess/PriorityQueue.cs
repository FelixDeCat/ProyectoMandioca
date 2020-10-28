using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    public enum Priority { high, low, med }

    Dictionary<Priority, List<T>> allcollections = new Dictionary<Priority, List<T>>();

    public void Enqueue(T val, Priority priority, bool _override = false)
    {
        if (!allcollections[priority].Contains(val))
        {
            allcollections[priority].Add(val);
        }
        else
        {
            if (_override)
            {
                for (int i = 0; i < allcollections[priority].Count; i++)
                {
                    if (allcollections[priority].Equals(val))
                    {

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

    }


    //public T Dequeue()
    //{

    //}



    /*
     
     entra 1, T con su priority
    selecciono la colleccion
    lo agrego

    tiene que tener remove

    tiene que tener...

    //parte de queue
    peek
    enqueue
    dequeue
    //parte de lista
    remove
    removeAt
     
     */

}
