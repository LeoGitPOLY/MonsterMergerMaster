using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int s_currency;

    // Monster Datas by Arena:
    public List<MonsterData> dataArena_1;
    public List<MonsterData> dataArena_2;
    public List<MonsterData> dataArena_3;
    public List<MonsterData> dataArena_4;
    public List<MonsterData> dataArena_final;

    //public List<MonsterData> mons

    public ScoreData(ScoreInstance score)
    {
        s_currency = score.currency;

        dataArena_1 = score.saveableMonsterDatas[0];
        dataArena_2 = score.saveableMonsterDatas[1];
        dataArena_3 = score.saveableMonsterDatas[2];
        dataArena_4 = score.saveableMonsterDatas[3];
        dataArena_final = score.saveableMonsterDatas[4];
    }
}