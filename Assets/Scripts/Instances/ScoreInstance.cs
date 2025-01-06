using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreInstance : MonoBehaviour
{
    public static ScoreInstance instance = null;

    public int currency;

    // Monster information:
    public List<MonsterData> saveableMonsterDatas;
    public int[] nbCurrentMonster; // NOT SAVED


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
        saveableMonsterDatas = PoolSystem.instance.getDataMonster();
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

        saveableMonsterDatas = new List<MonsterData>();
        nbCurrentMonster = new int[5];
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

        saveableMonsterDatas = data.monsterDatas;

        //Not saveable variable:
        nbCurrentMonster = new int[5];
    }

}
