using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public int level;
    public TypeMonster type;
    public float[] centeredPosition;
    public bool asHatched;

    public MonsterData(int newLevel, TypeMonster newType)
    {
        level = newLevel;
        type = newType;
        centeredPosition = new float[2];
        asHatched = true;
    }
    public MonsterData(int newLevel, TypeMonster newType, float[] newCenteredPosition)
    {
        level = newLevel;
        type = newType;
        centeredPosition = newCenteredPosition;
        asHatched = false;
    }
}

public class MonsterScript : MonoBehaviour
{
    public MonsterData data;
    public EasyComponentsGetter getter;

    public LocalTimer timerCurrency;
    public LocalTimer timerMov;

    private void Awake()
    {
        getter = GetComponent<EasyComponentsGetter>();
        
        timerCurrency = new LocalTimer();
        timerMov = new LocalTimer();
    }

    //METHODE TO BE REMOVE!! (Allows dragged Monster to work) ----> ONLY AFTER THE IF DATA
    private void Start()
    {
        timerCurrency = new LocalTimer(MonsterStaticScript.GetTimeGenerateCurrency(data));
        timerMov = new LocalTimer(MonsterStaticScript.GetTimeGenerateCurrency(data));

        if (data != null) { return; }

        data = new MonsterData(1, TypeMonster.MonsterGreen);
        MonsterStaticScript.setMonsterVisual(this);    
    }
    private void Update()
    {
        timerCurrency.update(Time.deltaTime);
        timerMov.update(Time.deltaTime);

        if(timerCurrency.isDone()) { MonsterStaticScript.GenerateCurrency(this); }
        if(timerMov.isDone()) { MonsterStaticScript.MoveAround(this); }
    }
}

