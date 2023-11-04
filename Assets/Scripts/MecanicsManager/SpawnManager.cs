using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    PrefabHolder holder;

    float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEvent.instance.onSpawnMonsterInArena += SpawnMonsterInArena;
        holder = PrefabHolder.instance;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > 1)
        {
            SpawnMonsterInArena(TypeMonster.MonsterRed, 1, 0);
            currentTime = 0;
        }
    }

    private void SpawnMonsterInArena(TypeMonster type, int level, int indexArena)
    {
        MonsterData mdata = new MonsterData(level, type, randomPositionArena(indexArena));

        GameObject monsterObj = Instantiate(holder.getMonsterPrefab(), holder.getGameObjectFromArena(indexArena, 2).transform);
        MonsterScript mscript = monsterObj.GetComponent<MonsterScript>();

        MonsterStaticScript.SetStatsMonster(mscript, mdata);
    }

    // Private logic methodes:
    private Vector2 randomPositionArena(int index)
    {
        Transform arenaTransform = holder.getGameObjectFromArena(index, 1).transform;
        Vector2 maxPos = arenaTransform.position + arenaTransform.localScale/2;
        Vector2 minPos = arenaTransform.position - arenaTransform.localScale/2;

        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);

        return new Vector2(randomX, randomY);
    }
}
