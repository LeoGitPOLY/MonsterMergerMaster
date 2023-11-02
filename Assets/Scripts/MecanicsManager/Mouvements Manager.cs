using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementsManager : MonoBehaviour
{
    private List<(MonsterStats, int)> monsterToDrags;
    private Camera cameraWorld;

    // Start is called before the first frame update
    void Start()
    {
        SelectionEvent.instance.onSetDragMonster += SetMonsterToDrag;
        SelectionEvent.instance.onDragMonster += DragMonster;
        cameraWorld = Camera.main;

        monsterToDrags = new List<(MonsterStats, int)>();
    }

    private void SetMonsterToDrag(MonsterStats monsterStats, int id, bool isAdded)
    {
        if (isAdded)
            monsterToDrags.Add((monsterStats, id));
        else
            monsterToDrags.Remove((monsterStats, id));
    }

    private void DragMonster(Vector2 newPositon, int id)
    {
        MonsterStats monsterToDrag = getMonsterStatsByID(id);
        if (monsterToDrag == null) { return; }

        newPositon = cameraWorld.ScreenToWorldPoint(newPositon);

        //Might want to add some smothing to that (if time)
        monsterToDrag.gameObject.transform.position = newPositon;
    }

    private MonsterStats getMonsterStatsByID(int id)
    {
        foreach ((MonsterStats, int) item in monsterToDrags)
        {
            if (item.Item2 == id)
                return item.Item1;
        }
        return null;
    }
}
