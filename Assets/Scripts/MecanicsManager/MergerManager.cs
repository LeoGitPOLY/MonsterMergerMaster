using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerManager : MonoBehaviour
{
    [Header("VARIABLES:")]
    [SerializeField] private float detectionRadius;


    private void Start()
    {
        SelectionEvent.instance.onSetDragMonster += DropedMonster;
    }

    private void DropedMonster(MonsterScript monsterStats, int id, bool isAdded)
    {
        //Only detect on release
        if (isAdded) { return; }

        Vector3 droppedPos = monsterStats.gameObject.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(droppedPos, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            MonsterScript otherMonster = collider.gameObject.GetComponent<MonsterScript>();
            if (otherMonster != null && otherMonster != monsterStats)
            {
                MergeMonster(monsterStats, otherMonster);
                return;
            }
        }
    }

    public void MergeMonster(MonsterScript monster1, MonsterScript monster2)
    {
        if (monster1 == null || monster2 == null) { return; }

        //VERIFICATION: Monster same level
        if (!MonsterStaticScript.IsSameLevel(monster1, monster2)) { return; }

        //VERIFICATION: Monster same type
        if (!MonsterStaticScript.IsSameType(monster1, monster2)) { return; }

        MonsterStaticScript.Upgrade(monster1);
        MonsterStaticScript.DeleteMonster(monster2);
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
