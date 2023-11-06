using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public int level;
    public TypeMonster type;
    public float[] centeredPosition;

    public MonsterData(int newLevel, TypeMonster newType)
    {
        level = newLevel;
        type = newType;
        centeredPosition = new float[2];
    }
    public MonsterData(int newLevel, TypeMonster newType, float[] newCenteredPosition)
    {
        level = newLevel;
        type = newType;
        centeredPosition = newCenteredPosition;
    }
}

public class MonsterScript : MonoBehaviour
{
    public MonsterData data;
    public EasyComponentsGetter getter;

    private void Awake()
    {
        getter = GetComponent<EasyComponentsGetter>();
    }
    //METHODE TO BE REMOVE!! (Allows dragged Monster to work)
    private void Start()
    {
        if (data != null) { return; }

        data = new MonsterData(1, TypeMonster.MonsterGreen);
        MonsterStaticScript.setMonsterVisual(this);
    }
    private void Update()
    {
        // Timer pour la generation d'argent
        // Timer pour ...
    }
}

