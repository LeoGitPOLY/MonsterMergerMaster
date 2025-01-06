using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTimer
{
    private float currentTime;
    private float maxTime;

    public LocalTimer(float maxTime = 0)
    {
        this.currentTime = 0;
        this.maxTime = maxTime;
    }

    public void update(float time)
    {
        currentTime += time;
    }
    public bool isDone()
    {
        bool isDone = false;
        if (currentTime >= maxTime)
        {
            currentTime = 0;
            isDone = true;
        }

        return isDone;
    }
    public void setNewMax(float time)
    {
        maxTime = time;
    }
}
