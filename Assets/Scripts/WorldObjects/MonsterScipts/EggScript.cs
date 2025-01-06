using UnityEngine;

public class EggScript : MonoBehaviour
{
    public MonsterData data;
    public EasyComponentsGetter getter;
    public Animator anim;

    public int numberOfTap;

    private void Awake()
    {
        getter = GetComponent<EasyComponentsGetter>();
        anim = GetComponent<Animator>();
    }

    //METHODE TO BE REMOVE!! (Allows dragged Monster to work) ----> ONLY AFTER THE IF DATA
    private void Start()
    {
        numberOfTap = 0;
        data.asHatched = false;

        if (data != null) { return; }

        data = new MonsterData(1, TypeMonster.MonsterGreen);
        data.asHatched = false;
    }

    private void Update()
    { 
    }
}

