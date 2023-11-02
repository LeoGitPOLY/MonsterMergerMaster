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
    public event Action<MonsterStats> onSelectionMonster;
    public void SelectionMonster(MonsterStats monsterStats)
    {
        onSelectionMonster?.Invoke(monsterStats);
    }

    public event Action<MonsterStats, int, bool> onSetDragMonster;
    public void SetDragMonster(MonsterStats monsterStats, int id, bool isAdded)
    {
        onSetDragMonster?.Invoke(monsterStats, id, isAdded);
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
