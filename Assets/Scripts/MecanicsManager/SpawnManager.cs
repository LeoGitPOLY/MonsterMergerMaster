using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    PrefabHolder holder;

    float currentTime = 0f;
    int numberSpawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEvent.instance.onSpawnMonsterInArena += SpawnMonsterInArena;
        holder = PrefabHolder.instance;

        SpawnAllMonsterStart();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1 && numberSpawn < 10)
        {
            /*SpawnMonsterInArena(TypeMonster.MonsterRed, 1, 0);
            SpawnMonsterInArena(TypeMonster.MonsterGreen, 1, 1);
            SpawnMonsterInArena(TypeMonster.MonsterBlue, 1, 2);
            SpawnMonsterInArena(TypeMonster.MonsterYellow, 1, 3);
            currentTime = 0;
            numberSpawn++;*/
        }
    }

    /// <summary>
    /// TO DELETEEEE
    /// </summary>
    public void callableMethode()
    {
        float startTime = Time.time;
        MonsterStaticScript.getMonsterDataFormWorld();
        float endTime = Time.time;

        Debug.Log("Function took: " + (endTime - startTime));
    }

    private void SpawnAllMonsterStart()
    {
        List<MonsterData>[] monsterDatas = ScoreInstance.instance.saveableMonsterDatas;

        for (int i = 0; i < monsterDatas.Length; i++)
        {
            foreach (var item in monsterDatas[i])
            {
                SpawnMonsterInArena(item);
            }
        }
    }
    private void SpawnMonsterInArena(MonsterData mdata)
    {
        const int indexMonsterContener = 2;
        GameObject monsterObj = Instantiate(holder.getMonsterPrefab(), holder.getGameObjectFromArena((int)mdata.type, indexMonsterContener).transform);
        MonsterScript mscript = monsterObj.GetComponent<MonsterScript>();

        MonsterStaticScript.SetStatsMonster(mscript, mdata);
    }

    private void SpawnMonsterInArena(TypeMonster type, int level, int indexArena)
    {
        const int indexMonsterContener = 2;
        MonsterData mdata = new MonsterData(level, type, randomPositionArena(indexArena));

        GameObject monsterObj = Instantiate(holder.getMonsterPrefab(), holder.getGameObjectFromArena(indexArena, indexMonsterContener).transform);
        MonsterScript mscript = monsterObj.GetComponent<MonsterScript>();

        MonsterStaticScript.SetStatsMonster(mscript, mdata);
    }

    // Private logic methodes:
    private float[] randomPositionArena(int index)
    {
        Transform arenaTransform = holder.getGameObjectFromArena(index, 1).transform;
        Vector2 maxPos = arenaTransform.position + arenaTransform.localScale / 2;
        Vector2 minPos = arenaTransform.position - arenaTransform.localScale / 2;

        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);

        return new float[] { randomX, randomY };
    }
}
