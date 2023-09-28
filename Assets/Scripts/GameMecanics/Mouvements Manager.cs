using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementsManager : MonoBehaviour
{
    [SerializeField] private float deadZone = 0.3f;

    private MonsterScript monsterToDrag;

    // Start is called before the first frame update
    void Start()
    {
        SelectionEvent.instance.onSetDragMonster += SetMonsterToDrag;
        SelectionEvent.instance.onDragMonster += DragMonster;
    }

    private void SetMonsterToDrag(MonsterScript monsterScript)
    {
        monsterToDrag = monsterScript;
    }

    private void DragMonster(Vector2 newPositon)
    {
        if (monsterToDrag == null)
            return;

        //if (!IsFingerClose(newPositon))
        //    return;

        //Might want to add some smothing to that (if time)
        monsterToDrag.gameObject.transform.position = newPositon;
    }

    private bool IsFingerClose(Vector2 newPosition) 
    {
        bool isLowerX = newPosition.x < monsterToDrag.transform.position.x + deadZone;
        bool isLowerY = newPosition.y < monsterToDrag.transform.position.y + deadZone;
        bool isUpperX = newPosition.x > monsterToDrag.transform.position.x - deadZone; 
        bool isUpperY = newPosition.y > monsterToDrag.transform.position.y - deadZone;


        return monsterToDrag.transform.position.x < newPosition.x + deadZone;
    }
}
