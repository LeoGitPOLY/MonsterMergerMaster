using UnityEngine;
using System;

public class SpawnEvent : MonoBehaviour
{
    public static SpawnEvent instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public event Action<TypeMonster, int, int> onSpawnMonsterInArena;
    public void SpawnMonsterInArena(TypeMonster type, int level, int indexArena)
    {
        onSpawnMonsterInArena?.Invoke(type, level, indexArena);
    }
}
