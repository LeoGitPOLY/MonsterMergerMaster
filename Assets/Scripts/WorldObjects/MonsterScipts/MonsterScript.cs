using UnityEngine;

public class MonsterData
{
    public int level;
    public TypeMonster type;
    public Vector2 centeredPosition;

    public MonsterData(int newLevel, TypeMonster newType)
    {
        level = newLevel;
        type = newType;
        centeredPosition = Vector2.zero;
    }
    public MonsterData(int newLevel, TypeMonster newType, Vector2 newCenteredPosition)
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

