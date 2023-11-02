using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerManager : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform parentMonster;

    [Header("VARIABLES:")]
    [SerializeField] private float detectionRadius;


    private void Start()
    {
        SelectionEvent.instance.onSetDragMonster += DropedMonster;
    }

    private void DropedMonster(MonsterStats monsterStats, int id, bool isAdded)
    {
        //Only detect on release
        if (isAdded) { return; }

        Vector3 droppedPos = monsterStats.gameObject.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(droppedPos, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            MonsterStats otherMonster = collider.gameObject.GetComponent<MonsterStats>();
            if (otherMonster != null && otherMonster != monsterStats)
            {
                MergeMonster(monsterStats, otherMonster);
                return;
            }
        }
    }

    public void MergeMonster(MonsterStats monster1, MonsterStats monster2)
    {
        if (monster1 == null || monster2 == null) { return; }

        //VERIFICATION: Monster same level
        if (!MonsterScript.IsSameLevel(monster1, monster2)) { return; }

        //VERIFICATION: Monster same type
        if (!MonsterScript.IsSameType(monster1, monster2)) { return; }

        MonsterScript.Upgrade(monster1);
        MonsterScript.DeleteMonster(monster2);
    }

    // WILL BE IN THE SPAWN MANAGER SCRIPT
    public void createMonster()
    {
        TypeMonster type = (TypeMonster)Random.Range(0, 4);


        MonsterStats newStats = new MonsterStats(1, type);
        GameObject newMonster = Instantiate(monsterPrefab, parentMonster);

        //newMonster.GetComponent<MonsterScript>().SetStats(newStats);
    }

    //OLD METHODE TO SELECT MONSTER (might use it later)
    /* private void newSelectionMonster(MonsterScript monsterScript)
     {
         if (monsterScript == selectionMonsters[0])
             return;

         selectionMonsters[1]?.setMonsterSelected(false);

         selectionMonsters[1] = selectionMonsters[0];
         selectionMonsters[0] = monsterScript;

         selectionMonsters[0]?.setMonsterSelected(true);
     }*/
}
