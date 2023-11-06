using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateStartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnEvent.instance.SpawnMonsterInArena(TypeMonster.MonsterRed, 4, 4);
    }


}
