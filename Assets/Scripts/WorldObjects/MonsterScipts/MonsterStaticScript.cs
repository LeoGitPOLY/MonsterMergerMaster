using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterStaticScript
{
    //MAIN LOGIC MONSTER FUNTIONS:
    public static void GenerateMoney(MonsterScript mScript)
    {
    }
    public static void setMonsterVisual(MonsterScript mScript)
    {
        EasyComponentsGetter getter = mScript.getter;
        TypeMonster type = mScript.data.type;
        int level = mScript.data.level;

        //Will be modify with the real skins
        getter.getGameObject(6).GetComponent<SpriteRenderer>().sprite = getter.getSprite(level - 1);
        getter.setActiveGameObject(2, type == TypeMonster.MonsterRed);
        getter.setActiveGameObject(3, type == TypeMonster.MonsterBlue);
        getter.setActiveGameObject(4, type == TypeMonster.MonsterGreen);
        getter.setActiveGameObject(5, type == TypeMonster.MonsterYellow);
    }
    public static void Upgrade(MonsterScript mScript)
    {
        mScript.data.level++;

        //Show the changes:
        setMonsterVisual(mScript);
    }


    // LOGIC MONSTER FUNCTIONS:
    public static void DeleteMonster(MonsterScript mScript)
    {
        Object.Destroy(mScript.gameObject);
    }
    public static void SetStatsMonster(MonsterScript mScript, MonsterData mData)
    {
        mScript.data = mData;

        //Show the changes:
        CenteredMonster(mScript, mScript.data.centeredPosition);
        setMonsterVisual(mScript);
    }
    public static void CenteredMonster(MonsterScript mScript, Vector2 newCenteredPosition)
    {
        mScript.data.centeredPosition = newCenteredPosition;
        mScript.gameObject.transform.position = mScript.data.centeredPosition;
    }


    // VERIFICATION MONSTER FUNCTIONS:
    public static bool IsSameLevel(MonsterScript m1Script, MonsterScript m2Script)
    {
        return m1Script.data.level == m2Script.data.level;
    }
    public static bool IsSameType(MonsterScript m1Stats, MonsterScript m2Stats)
    {
        return m1Stats.data.type == m2Stats.data.type;
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
