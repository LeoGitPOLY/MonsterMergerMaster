using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterScript
{
    //MAIN LOGIC MONSTER FUNTIONS:
    public static void GenerateMoney(MonsterStats mStats)
    {
    }
    public static void setMonsterVisual(MonsterStats mStats)
    {
        EasyComponentsGetter getter = mStats.getter;
        TypeMonster type = mStats.type;
        int level = mStats.level;

        //Will be modify with the real skins
        getter.getGameObject(6).GetComponent<SpriteRenderer>().sprite = getter.getSprite(level - 1);
        getter.setActiveGameObject(2, type == TypeMonster.MonsterRed);
        getter.setActiveGameObject(3, type == TypeMonster.MonsterBlue);
        getter.setActiveGameObject(4, type == TypeMonster.MonsterGreen);
        getter.setActiveGameObject(5, type == TypeMonster.MonsterYellow);
    }
    public static void Upgrade(MonsterStats mStats)
    {
        mStats.level++;

        //Show the changes:
        setMonsterVisual(mStats);
    }


    // LOGIC MONSTER FUNCTIONS:
    public static void DeleteMonster(MonsterStats mStats)
    {
        Object.Destroy(mStats.gameObject);
    }
    public static void SetStatsMonster(MonsterStats mStats, MonsterStats mStatsNew)
    {
        mStats.level = mStatsNew.level;
        mStats.type = mStatsNew.type;
        mStats.centeredPosition = mStatsNew.centeredPosition;

        //Show the changes:
        CenteredMonster(mStats, mStats.centeredPosition);
        setMonsterVisual(mStats);
    }
    public static void CenteredMonster(MonsterStats mStats, Vector2 newCenteredPosition)
    {
        mStats.centeredPosition = newCenteredPosition;
        mStats.gameObject.transform.position = mStats.centeredPosition;
    }


    // VERIFICATION MONSTER FUNCTIONS:
    public static bool IsSameLevel(MonsterStats m1Stats, MonsterStats m2Stats)
    {
        return m1Stats.level == m2Stats.level;
    }
    public static bool IsSameType(MonsterStats m1Stats, MonsterStats m2Stats)
    {
        return m1Stats.type == m2Stats.type;
    }


    // OLD METHODE: to merge different monster with different type
    public static List<TypeMonster> mergeType_OLD(List<TypeMonster> addedType1, List<TypeMonster> addedType2)
    {
        if (addedType1 == null || addedType2 == null)
            return null;

        foreach (TypeMonster item in addedType2)
        {
            if (!addedType1.Contains(item))
                addedType1.Add(item);
        }

        return addedType1;
    }

}
