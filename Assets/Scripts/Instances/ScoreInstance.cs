using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreInstance : MonoBehaviour
{
    public static ScoreInstance instance = null;

    public int currency;
    public List<MonsterData>[] saveableMonsterDatas;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }

    // CURRENCY METHODE:
    public void addCurrency(int currencyAmount)
    {
        currency += currencyAmount;
        //EventsGameScore.instance.changeGold();
    }
    public bool payCurrency(int currencyAmount)
    {
        bool enoughtCurrency = haveEnoughtCurrency(currencyAmount);

        if (enoughtCurrency)
        {
            currency -= currencyAmount;
        }
        return enoughtCurrency;
    }
    public bool haveEnoughtCurrency(int currencyAmount)
    {
        bool enoughtCurrency = currencyAmount <= currency;

        if (!enoughtCurrency)
        {
            //UIManagerPop.instance.setnotEnoughtMoney();
        }

        return enoughtCurrency;
    }

   
    //SAVE METHODE:
    public void Save()
    {
        saveableMonsterDatas = MonsterStaticScript.getMonsterDataFormWorld();
        SaveSystem.saveScore(instance);
    }

    public void Load(EnumVersion version)
    {
        switch (version)
        {
            case EnumVersion.newVersion:
                loadNew();
                break;
            case EnumVersion.initialVersion_0_0:
                loadVersion_Initiale_0_0();
                break;
            default:
                Debug.LogError("THIS VERSION DOESN'T EXIST (!)");
                break;
        }

    }
    public void loadNew()
    {
        currency = 100;

        saveableMonsterDatas = new List<MonsterData>[5];
        saveableMonsterDatas[0] = new List<MonsterData>();
        saveableMonsterDatas[1] = new List<MonsterData>();
        saveableMonsterDatas[2] = new List<MonsterData>();
        saveableMonsterDatas[3] = new List<MonsterData>();
        saveableMonsterDatas[4] = new List<MonsterData>();
    }
    public void loadDevelloperMode()
    {
        currency = 1000000;
        
    }

    /*
     * LOAD PRIVATE (care about the version) :
     */
    private void loadVersion_Initiale_0_0()
    {
        ScoreData data = SaveSystem.loadScore();

        currency = data.s_currency;

        saveableMonsterDatas = new List<MonsterData>[5];
        saveableMonsterDatas[0] = data.dataArena_1;
        saveableMonsterDatas[1] = data.dataArena_2;
        saveableMonsterDatas[2] = data.dataArena_3;
        saveableMonsterDatas[3] = data.dataArena_4;
        saveableMonsterDatas[4] = data.dataArena_final;
    }

}
