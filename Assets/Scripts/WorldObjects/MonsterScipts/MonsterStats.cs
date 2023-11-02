using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public int level;
    public TypeMonster type;
    public Vector2 centeredPosition;
    public EasyComponentsGetter getter;
   
    //METHODE TO BE REMOVE!! (Allows dragged Monster to work)
    private void Start()
    {
        level = 1;
        type = TypeMonster.MonsterRed;
        getter = GetComponent<EasyComponentsGetter>();
        MonsterScript.setMonsterVisual(this);
    }

    public MonsterStats(int newLevel = 1, TypeMonster newType = TypeMonster.MonsterRed)
    {
        level = newLevel;
        type = newType;
        centeredPosition = Vector2.zero;
        getter = GetComponent<EasyComponentsGetter>();
    }

}
