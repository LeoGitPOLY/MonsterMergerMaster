using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int s_currency;

    public ScoreData(ScoreInstance score)
    {
        s_currency = score.currency;
    }
}