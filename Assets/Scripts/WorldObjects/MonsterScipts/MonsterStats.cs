using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats
{
    private int level;
    private List<TypeMonster> types;

    public MonsterStats(int newLevel = 1, List<TypeMonster> newTypes = null )
    {
        level = newLevel;
        types = new List<TypeMonster>();

        types = mergeType(types, newTypes);
    }


    public int getLevel()
    {
        return level;
    }

    public List<TypeMonster> GetTypeMonsters()
    {
        return types;
    }

    public bool containType(TypeMonster type)
    {
        return types.Contains(type);
    }
    public static List<TypeMonster> mergeType(List<TypeMonster> addedType1, List<TypeMonster> addedType2)
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
