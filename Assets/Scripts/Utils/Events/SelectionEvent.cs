using UnityEngine;
using System;

public class SelectionEvent : MonoBehaviour
{
    public static SelectionEvent instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    //MONSTER EVENTS
    public event Action<MonsterScript> onSelectionMonster;
    public void SelectionMonster(MonsterScript monsterScript)
    {
        onSelectionMonster?.Invoke(monsterScript);
    }

    public event Action<MonsterScript, int, bool> onSetDragMonster;
    public void SetDragMonster(MonsterScript monsterScript, int id, bool isAdded)
    {
        onSetDragMonster?.Invoke(monsterScript, id, isAdded);
    }

    public event Action<Vector2, int> onDragMonster;
    public void DragMonster(Vector2 newPosition, int id)
    {
        onDragMonster?.Invoke(newPosition, id);
    }


    //BACK GROUND EVENTS
    public event Action<int, bool> onSetDragBackground;
    public void SetDragBackground(int id, bool isAdded)
    {
        onSetDragBackground?.Invoke(id, isAdded);
    }

    public event Action<int> onDragBackground;
    public void DragBackground(int id)
    {
        onDragBackground?.Invoke(id);
    }
}
