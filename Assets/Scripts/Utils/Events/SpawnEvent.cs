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

}
