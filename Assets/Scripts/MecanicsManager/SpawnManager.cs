using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    float currentTime = 0f;

    // Instance:
    private PrefabHolder prefFolder;
    private PoolSystem poolSystem;

    void Start()
    {
        SpawnEvent.instance.onSpawnMonsterInArena += SpawnMonsterInArena;
        SpawnEvent.instance.onSpawnEggInArena += SpawnEggInArena;
        SelectionEvent.instance.onClickEgg += ClickOnEgg;

        prefFolder = PrefabHolder.instance;
        poolSystem = PoolSystem.instance;

        SpawnAllEntityStart();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1)
        {
            int level = Random.Range(1, 4);
            SpawnEggInArena(TypeMonster.MonsterRed, level, 0);
            SpawnEggInArena(TypeMonster.MonsterGreen, 1, 1);
            SpawnEggInArena(TypeMonster.MonsterBlue, 1, 2);
            SpawnEggInArena(TypeMonster.MonsterYellow, 1, 3);
            currentTime = 0;
        }
    }

    private void SpawnAllEntityStart()
    {
        List<MonsterData> monsterDatas = ScoreInstance.instance.saveableMonsterDatas;
        Debug.LogError("Monster data load : " + monsterDatas.Count);

        foreach (var item in monsterDatas)
        {
            //Debug.LogError("ITEM: " + item.type);
            if (item.asHatched)
                SpawnMonsterInArena(item);
            else
                SpawnEggInArena(item);
        }
    }

    // Private GameObjects Spawn
    private void SpawnMonsterInArena(MonsterData mdata)
    {
        if (ScoreInstance.instance.nbCurrentMonster[(int)mdata.type] >= VariableHolder.MAX_MONSTER_ARENA) { return; }
        MonsterStaticScript.InstantiateMonster(mdata);
    }
    private void SpawnMonsterInArena(TypeMonster type, int level, int indexArena)
    {
        MonsterData mdata = new MonsterData(level, type, randomPositionArena(indexArena));
        SpawnMonsterInArena(mdata);
    }
    private void SpawnEggInArena(MonsterData edata)
    {
        if (ScoreInstance.instance.nbCurrentMonster[(int)edata.type] >= VariableHolder.MAX_MONSTER_ARENA) { return; }
        MonsterStaticScript.InstantiateEgg(edata);
    }
    private void SpawnEggInArena(TypeMonster type, int level, int indexArena)
    {
        MonsterData edata = new MonsterData(level, type, randomPositionArena(indexArena));
        edata.asHatched = false;

        SpawnEggInArena(edata);
    }

    // Private Hatch Spawn
    private IEnumerator HatchEgg(EggScript eScript, float time)
    {
        yield return new WaitForSeconds(time);

        MonsterData data = eScript.data;
        data.asHatched = true;

        MonsterStaticScript.DeleteEgg(eScript);
        SpawnMonsterInArena(data);
    }


    // Private logic methodes:
    private float[] randomPositionArena(int index)
    {
        Transform arenaTransform = prefFolder.getGameObjectFromArena(index, 1).transform;
        Vector2 maxPos = arenaTransform.position + arenaTransform.localScale / 2;
        Vector2 minPos = arenaTransform.position - arenaTransform.localScale / 2;

        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);

        return new float[] { randomX, randomY };
    }
    private void ClickOnEgg(EggScript eScript)
    {
        if (eScript.numberOfTap + 1 > eScript.data.level) { return; }

        eScript.numberOfTap++;
        float timeAnim = MonsterStaticScript.AnimAndGetTime(eScript);

        if (eScript.numberOfTap == eScript.data.level)
        {
            StartCoroutine(HatchEgg(eScript, timeAnim));
        }
    }
}
