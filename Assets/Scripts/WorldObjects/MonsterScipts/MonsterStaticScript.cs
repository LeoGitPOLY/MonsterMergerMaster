using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterStaticScript
{
    private static GameObject mObject = PrefabHolder.instance.getMonsterPrefab();
    private static GameObject eObject = PrefabHolder.instance.getEggPrefab();

    //MAIN LOGIC MONSTER FUNTIONS:
    public static void GenerateCurrency(MonsterScript mScript)
    {
        int arenaIndex = (int)mScript.data.type;

        //if (arenaIndex != 0) { return; }

        GameObject currencyPrefab = PrefabHolder.instance.getCurrencyFromArena(arenaIndex);
        Transform parent = PrefabHolder.instance.getGameObjectFromArena(arenaIndex, 3).transform;
        Transform posMonster = mScript.getter.getGameObject(7).transform;

        PoolObject currency = PoolSystem.instance.InstantiatePool(currencyPrefab,TypePool.CurrencyPool, parent);
        currency.function = () => ScoreInstance.instance.addCurrency(1);

        //Set Position:
        currency.obj.transform.position = posMonster.position;

        // Random Direction:
        int randomDirection = Random.Range(0, 2) * 2 - 1;
        Vector3 direction = currency.obj.transform.localScale;
        direction.x *= randomDirection;
        currency.obj.transform.localScale = direction;
    }
    public static void MoveAround(MonsterScript mScript)
    {
    }
    public static void setMonsterVisual(MonsterScript mScript)
    {
        EasyComponentsGetter getter = mScript.getter;
        TypeMonster type = mScript.data.type;
        int level = mScript.data.level;

        //Will be modify with the real skins
        getter.getGameObject(6).GetComponent<SpriteRenderer>().sprite = getter.getSprite(level - 1);
        getter.setActiveGameObject(2, type == TypeMonster.MonsterRed);
        getter.setActiveGameObject(3, type == TypeMonster.MonsterBlue);
        getter.setActiveGameObject(4, type == TypeMonster.MonsterGreen);
        getter.setActiveGameObject(5, type == TypeMonster.MonsterYellow);
    }
    public static void Upgrade(MonsterScript mScript)
    {
        mScript.data.level++;

        //Show the changes:
        setMonsterVisual(mScript);
    }


    // LOGIC MONSTER FUNCTIONS:
    public static void InstantiateMonster(MonsterData mData)
    {
        PoolObject monsterPool = PoolSystem.instance.InstantiatePool(
            mObject,
            TypePool.MonsterPool,
            PrefabHolder.instance.getGameObjectFromArena((int)mData.type, 2).transform);

        MonsterScript mscript = monsterPool.obj.GetComponent<MonsterScript>();
        SetStatsMonster(mscript, mData);

        ScoreInstance.instance.nbCurrentMonster[(int)mData.type]++;
    }
    public static void DeleteMonster(MonsterScript mScript)
    {
        PoolSystem.instance.DestroyPool(mScript.gameObject);
        ScoreInstance.instance.nbCurrentMonster[(int)mScript.data.type]--;
    }
    public static void SetStatsMonster(MonsterScript mScript, MonsterData mData)
    {
        mScript.data = mData;

        //Show the changes:
        CenteredMonster(mScript, mScript.data.centeredPosition);
        setMonsterVisual(mScript);
    }
    public static void CenteredMonster(MonsterScript mScript, float[] newCenteredPosition)
    {
        mScript.data.centeredPosition = newCenteredPosition;
        mScript.gameObject.transform.position = new Vector2(newCenteredPosition[0], newCenteredPosition[1]);
    }
    public static float GetTimeGenerateCurrency(MonsterData mData)
    {
        return 2f * Random.Range(1, 1.5f);
    }
    public static float GetTimeMoveAround(MonsterData mData)
    {
        return 1.1f;
    }

    // LOGIC EGG FUNCTIONS:
    public static void InstantiateEgg(MonsterData eData)
    {
        PoolObject eggPool = PoolSystem.instance.InstantiatePool(
            eObject,
            TypePool.EggPool,
            PrefabHolder.instance.getGameObjectFromArena((int)eData.type, 2).transform);

        EggScript escript = eggPool.obj.GetComponent<EggScript>();
        SetStatsEgg(escript, eData);

        ScoreInstance.instance.nbCurrentMonster[(int)eData.type]++;
    }
    public static void DeleteEgg(EggScript eScript)
    {
        PoolSystem.instance.DestroyPool(eScript.gameObject);
        ScoreInstance.instance.nbCurrentMonster[(int)eScript.data.type]--;
    }
    public static void SetStatsEgg(EggScript eScript, MonsterData mData)
    {
        eScript.data = mData;
        eScript.numberOfTap = 0;

        //Show the changes:
        CenteredEgg(eScript, eScript.data.centeredPosition);
        AnimAndGetTime(eScript);
    }
    public static void CenteredEgg(EggScript mScript, float[] newCenteredPosition)
    {
        mScript.data.centeredPosition = newCenteredPosition;
        mScript.gameObject.transform.position = new Vector2(newCenteredPosition[0], newCenteredPosition[1]);
    }
    public static float AnimAndGetTime(EggScript mScript)
    {      
        const string ANIM_NAME = "Egg_";
        string fullName = ANIM_NAME + mScript.numberOfTap.ToString();

        AnimationClip[] clips = mScript.anim.runtimeAnimatorController.animationClips;
        AnimationClip targetClip = System.Array.Find(clips, clip => clip.name == fullName);

        mScript.anim.Play(fullName);

        return targetClip.length;
    }

    // VERIFICATION MONSTER FUNCTIONS:
    public static bool IsSameLevel(MonsterScript m1Script, MonsterScript m2Script)
    {
        return m1Script.data.level == m2Script.data.level;
    }
    public static bool IsSameType(MonsterScript m1Stats, MonsterScript m2Stats)
    {
        return m1Stats.data.type == m2Stats.data.type;
    }

    // UTILS FUNCTIONS:
    public static List<MonsterData>[] getMonsterDataFormWorld()
    {
        const int indexMonsterContener = 2;
        List<MonsterData>[] monstersData = new List<MonsterData>[5];

        for (int i = 0; i < monstersData.Length; i++)
        {
            monstersData[i] = new List<MonsterData>();
            GameObject monsterContener = PrefabHolder.instance.getGameObjectFromArena(i, indexMonsterContener);
            MonsterScript[] monsterScripts =  monsterContener.GetComponentsInChildren<MonsterScript>();
            EggScript[] eggScripts =  monsterContener.GetComponentsInChildren<EggScript>();

            foreach (var item in monsterScripts)
            {
                monstersData[i].Add(item.data);
            }

            foreach (var item in eggScripts)
            {
                monstersData[i].Add(item.data);
            }
        }

/*        for (int i = 0; i < monstersData.Length; i++)
        {
            foreach (var item in monstersData[i])
            {
                Debug.Log("Monster: " + item.type.ToString() + " Level: " + item.level);
            }
        }*/
        return monstersData;
    }
}
