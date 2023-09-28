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

    public event Action<MonsterScript> onSelectionMonster;
    public void SelectionMonster(MonsterScript monsterScript)
    {
        onSelectionMonster?.Invoke(monsterScript);
    }

    public event Action<MonsterScript> onSetDragMonster;
    public void SetDragMonster(MonsterScript monsterScript)
    {
        onSetDragMonster?.Invoke(monsterScript);
    }

    public event Action<Vector2> onDragMonster;
    public void DragMonster(Vector2 newPosition)
    {
        onDragMonster?.Invoke(newPosition);
    }

    public event Action<Vector2, bool> onDragBackground;
    public void DragBackground(Vector2 newPos, bool isDragable)
    {
        onDragBackground?.Invoke(newPos, isDragable);
    }
}
