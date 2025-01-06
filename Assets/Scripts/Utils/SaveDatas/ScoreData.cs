using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int s_currency;

    // Monster Datas by Arena:
    public List<MonsterData> monsterDatas;

    //public List<MonsterData> mons

    public ScoreData(ScoreInstance score)
    {
        s_currency = score.currency;

        monsterDatas = score.saveableMonsterDatas;
    }
}