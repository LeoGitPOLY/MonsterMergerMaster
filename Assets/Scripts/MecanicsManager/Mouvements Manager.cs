using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementsManager : MonoBehaviour
{
    private List<(MonsterScript, int)> monsterToDrags;
    private Camera cameraWorld;

    private Vector2 maxPosClamp;
    private Vector2 minPosClamp;

    // Start is called before the first frame update
    void Start()
    {
        SelectionEvent.instance.onSetDragMonster += SetMonsterToDrag;
        SelectionEvent.instance.onDragMonster += DragMonster;
        ArenaEvent.instance.onChangeArena += ArenaChangeCall;
        cameraWorld = Camera.main;

        monsterToDrags = new List<(MonsterScript, int)>();
    }

    // METHODES CALLED BY EVENTS:
    private void SetMonsterToDrag(MonsterScript monsterScript, int id, bool isAdded)
    {
        if (isAdded)
            monsterToDrags.Add((monsterScript, id));
        else
        {
            setMonsterDataPosition(monsterScript);
            monsterToDrags.Remove((monsterScript, id));
        }
    }
    private void DragMonster(Vector2 newPositon, int id)
    {
        MonsterScript monsterToDrag = getMonsterScriptByID(id);
        if (monsterToDrag == null) { return; }

        newPositon = cameraWorld.ScreenToWorldPoint(newPositon);

        float x = Mathf.Clamp(newPositon.x, minPosClamp.x, maxPosClamp.x);
        float y = Mathf.Clamp(newPositon.y, minPosClamp.y, maxPosClamp.y);

        monsterToDrag.gameObject.transform.position = new Vector2(x, y);
    }
    private void ArenaChangeCall(int newArenaIndex, bool callback)
    {
        if (callback)
        {
            // New Arena reached: Set the new bounderies for monsters
            Transform arenaTransform = PrefabHolder.instance.getGameObjectFromArena(newArenaIndex, 1).transform;

            maxPosClamp = arenaTransform.position + arenaTransform.localScale / 2;
            minPosClamp = arenaTransform.position - arenaTransform.localScale / 2;
        }
        else
        {
            // Request to switch Arena: Drop all monsters
            monsterToDrags.Clear();
        }
    }
    

    // PRIVATE LOGIC METHODES:
    private void setMonsterDataPosition(MonsterScript monsterScript)
    {
        Vector2 gameObjPos = monsterScript.gameObject.transform.position;
        float[] centeredPos = new float[2] {gameObjPos.x, gameObjPos.y };

        MonsterStaticScript.CenteredMonster(monsterScript, centeredPos);
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
