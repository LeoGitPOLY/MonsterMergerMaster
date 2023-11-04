using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private const int MAX_INDEX = 3;

    private int wantedIndex;
    private int currentIndex;

    private bool bufferFullCamera;

    //Reference vers instance:
    private PrefabHolder prefabHolder;
    private ArenaEvent arenaEvent;

    // Start is called before the first frame update
    void Start()
    {
        //Need to be load at one point
        wantedIndex = 0;
        currentIndex = 0;
        bufferFullCamera = false;

        //Instance:
        prefabHolder = PrefabHolder.instance;
        arenaEvent = ArenaEvent.instance;

        //Listeners:
        arenaEvent.onBufferFullChangeArena += ArenaBufferFull;
        arenaEvent.onChangeArena += callbackCamera;
    }
 
    private void loadNewFarm()
    {
        arenaEvent.ChangeArena(wantedIndex, callback: false);
    }

    //Public callable fonctions:
    public void nextFarm(bool isRight)
    {
        if (isRight && wantedIndex == MAX_INDEX) { return; }
        if (!isRight && wantedIndex == 0) { return; }
        if (bufferFullCamera) { return; }

        wantedIndex = isRight ? wantedIndex + 1 : wantedIndex - 1;
        loadNewFarm();
    }

    public void SelectArenaFarm(int index)
    {
        // VERIFICATION: On a un index different
        if (index == wantedIndex) { return; }
        if (bufferFullCamera) { return; }

        wantedIndex = index;
        loadNewFarm();
    }

    // Listener function:
    public void ArenaBufferFull(bool isFull)
    {
        bufferFullCamera = isFull;
    }

    public void callbackCamera(int newIndex, bool callback)
    {
        if (!callback) { return; }
        currentIndex = newIndex;
    }
}