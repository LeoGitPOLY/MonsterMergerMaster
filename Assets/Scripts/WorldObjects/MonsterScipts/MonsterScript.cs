using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    private MonsterStats stats;
    private EasyComponentsGetter getter;

    private void Awake()
    {
        getter = GetComponent<EasyComponentsGetter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stats == null)
            setNewInfoMonster(new MonsterStats(1, new List<TypeMonster>() { TypeMonster.MonsterRed }));
    }

    public void setMonsterSelected(bool isSelected)
    {
        getter.getGameObject(0).SetActive(isSelected);
    }

    public void setNewInfoMonster(MonsterStats newStats)
    {
        stats = newStats;

        //Will be modify with the real skins
        getter.getGameObject(6).GetComponent<SpriteRenderer>().sprite = getter.getSprite(newStats.getLevel()-1);
        getter.setActiveGameObject(2, newStats.containType(TypeMonster.MonsterRed));
        getter.setActiveGameObject(3, newStats.containType(TypeMonster.MonsterBlue));
        getter.setActiveGameObject(4, newStats.containType(TypeMonster.MonsterGreen));
        getter.setActiveGameObject(5, newStats.containType(TypeMonster.MonsterYellow));
    }

    public bool MonsterIsSameLevel(MonsterScript otherMonster)
    {
        return stats.getLevel() == otherMonster.stats.getLevel();
    }

    public MonsterStats getMonsterStats()
    {
        return stats;
    }

    public void deleteItSelf()
    {
        Destroy(this.gameObject);
    }

}
