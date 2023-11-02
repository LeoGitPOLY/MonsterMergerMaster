using System;
using UnityEngine;

public class ArenaEvent : MonoBehaviour
{
    public static ArenaEvent instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public event Action<int, bool> onChangeArena;
    public void ChangeArena(int newIndex, bool callback)
    {
        onChangeArena?.Invoke(newIndex, callback);
    }

    public event Action<bool> onBufferFullChangeArena;
    public void BufferFullChangeArena(bool isFull)
    {
        onBufferFullChangeArena?.Invoke(isFull);
    }
}

