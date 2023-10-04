using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementsManager : MonoBehaviour
{
    private List<(MonsterScript, int)> monsterToDrags;
    int i = 0;
    private Camera cameraWorld;

    // Start is called before the first frame update
    void Start()
    {
        SelectionEvent.instance.onSetDragMonster += SetMonsterToDrag;
        SelectionEvent.instance.onDragMonster += DragMonster;
        cameraWorld = Camera.main;

        monsterToDrags = new List<(MonsterScript, int)>();
    }

    private void SetMonsterToDrag(MonsterScript monsterScript, int id, bool isAdded)
    {
        if (isAdded)
            monsterToDrags.Add((monsterScript, id));
        else
            monsterToDrags.Remove((monsterScript, id));
    }

    private void DragMonster(Vector2 newPositon, int id)
    {
        MonsterScript monsterToDrag = getMonsterScriptByID(id);
        if (monsterToDrag == null) { return; }

        newPositon = cameraWorld.ScreenToWorldPoint(newPositon);

        //Might want to add some smothing to that (if time)
        monsterToDrag.gameObject.transform.position = newPositon;
    }

    private MonsterScript getMonsterScriptByID(int id)
    {
        foreach ((MonsterScript, int) item in monsterToDrags)
        {
            if (item.Item2 == id)
                return item.Item1;
        }
        return null;
    }
}
