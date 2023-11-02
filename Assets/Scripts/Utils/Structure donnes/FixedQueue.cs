using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedQueue<T>
{
    private T[] array;
    private int capacity;
    private int count;
    private bool forcedDequeueFlag;

    public FixedQueue(int capacity)
    {
        this.capacity = capacity;
        this.array = new T[capacity];
        this.count = 0;
        this.forcedDequeueFlag = false;
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public bool IsFull()
    {
        return count == capacity;
    }


    public bool getResetForcedFlag()
    {
        bool returnValue = forcedDequeueFlag;
        forcedDequeueFlag = false;

        return returnValue;
    }

    public void setForcedFlag()
    {
        forcedDequeueFlag = true;
    }


    public void Enqueue(T item)
    {
        if (count < capacity)
        {
            array[count] = item;
            count++;
        }
    }

    public T Dequeue()
    {
        if (IsEmpty()) { return default(T); }
        T item = array[0];

        for (int i = 1; i < capacity; i++)
        {
            array[i - 1] = array[i];
        }

        array[capacity - 1] = default(T);
        count--;

        return item;
    }

    public string printFixedQueue()
    {
        string toPrint = "[";
        foreach (var item in array)
        {
            toPrint += item.ToString() + ", ";
        }

        return (toPrint + "]");
    }

    public T seeFirstElem()
    {
        return array[0];
    }
}
