using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static bool countains(float value, float maxValue, float minValue)
    {
        return value <= maxValue && value >= minValue;
    }
    public static bool countainsInterval(float value, float centerValue, float interval)
    {
        return countains(value, centerValue + interval, centerValue -interval);
    }
}
