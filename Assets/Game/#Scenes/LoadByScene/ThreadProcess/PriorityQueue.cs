using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    public enum Priority { error ,high, low, med }

    Dictionary<Priority, Queue<T>> elements = new Dictionary<Priority, Queue<T>>();

    public PriorityQueue()
    {
        elements.Add(Priority.high, new Queue<T>());
        elements.Add(Priority.med, new Queue<T>());
        elements.Add(Priority.low, new Queue<T>());
    }

    public void Add_ElementByPriority(T obj, Priority priority)
    {
        elements[priority].Enqueue(obj);
    }

    public T Pull()
    {
        Priority pr = Priority.error;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[IndexToPriority(i)].Count > 0)
            {
                pr = IndexToPriority(i);
                break;
            }
        }
        if (pr == Priority.error) { throw new Exception(); }
        return elements[pr].Dequeue();
    }

    private Priority IndexToPriority(int index)
    {
        if (index == 0) return Priority.high;
        if (index == 1) return Priority.med;
        if (index == 2) return Priority.low;
        return Priority.error;
    }


}
