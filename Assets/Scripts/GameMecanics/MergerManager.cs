using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerManager : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform parentMonster;

    private MonsterScript[] selectionMonsters = new MonsterScript[2];


    private void Start()
    {
        SelectionEvent.instance.onSelectionMonster += newSelectionMonster;
    }

    private void newSelectionMonster(MonsterScript monsterScript)
    {
        if (monsterScript == selectionMonsters[0])
            return;

        selectionMonsters[1]?.setMonsterSelected(false);

        selectionMonsters[1] = selectionMonsters[0];
        selectionMonsters[0] = monsterScript;

        selectionMonsters[0]?.setMonsterSelected(true);
    }

    public void MergeMonster()
    {
        MonsterScript monster1 = selectionMonsters[0];
        MonsterScript monster2 = selectionMonsters[1];

        if (monster1 == null || monster2 == null)
            return;

        if (!monster1.MonsterIsSameLevel(monster2))
            return;

        int newLevel = monster1.getMonsterStats().getLevel() + 1;
        List<TypeMonster> newType = MonsterStats.mergeType(monster1.getMonsterStats().GetTypeMonsters(), monster2.getMonsterStats().GetTypeMonsters());
        MonsterStats newStats = new MonsterStats(newLevel, newType);

        GameObject newMonster = Instantiate(monsterPrefab, parentMonster);
        newMonster.GetComponent<MonsterScript>().setNewInfoMonster(newStats);

        monster1.deleteItSelf();
        monster2.deleteItSelf();

        selectionMonsters[0] = null;
        selectionMonsters[1] = null;
    }

    public void createMonster()
    {
        List<TypeMonster> type = new List<TypeMonster>();
        int randomEvo = Random.Range(0, 4);
        type.Add((TypeMonster)randomEvo);


        MonsterStats newStats = new MonsterStats(1, type);

        GameObject newMonster = Instantiate(monsterPrefab, parentMonster);
        newMonster.GetComponent<MonsterScript>().setNewInfoMonster(newStats);
    }
}
