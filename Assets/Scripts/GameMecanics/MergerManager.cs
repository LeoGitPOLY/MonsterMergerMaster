using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerManager : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform parentMonster;
    private const string TAG_CLICK = "Clickable";

    [Header("VARIABLES:")]
    [SerializeField] private float detectionRadius;

    private MonsterScript selectedMonster;

    private void Start()
    {
        SelectionEvent.instance.onSetDragMonster += DropedMonster;
    }

    private void DropedMonster(MonsterScript monsterScript, int id, bool isAdded)
    {
        //Only detect on release
        if (isAdded) { return; }

        Vector3 droppedPos = monsterScript.gameObject.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(droppedPos, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            MonsterScript otherMonster = collider.gameObject.GetComponent<MonsterScript>();
            if (otherMonster != null && otherMonster != monsterScript)
            {
                MergeMonster(monsterScript, otherMonster);
                return;
            }
        }
    }

    public void MergeMonster(MonsterScript monster1, MonsterScript monster2)
    {
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
    //OLD METHODE TO SELECT MONSTER (might use it later)
    private void newSelectionMonster(MonsterScript monsterScript)
    {
        //if (monsterScript == selectionMonsters[0])
        //    return;

        //selectionMonsters[1]?.setMonsterSelected(false);

        //selectionMonsters[1] = selectionMonsters[0];
        //selectionMonsters[0] = monsterScript;

        //selectionMonsters[0]?.setMonsterSelected(true);
    }
}
