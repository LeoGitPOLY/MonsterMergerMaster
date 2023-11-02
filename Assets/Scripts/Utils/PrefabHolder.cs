using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    //Main instance:
    public static PrefabHolder instance;

    [Header("Prefab Arena:")]
    [SerializeField] private List<GameObject> prefabArena;

    private List<EasyComponentsGetter> getter;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        getter = new List<EasyComponentsGetter>();

        foreach (GameObject item in prefabArena)
        {
            getter.Add(item.GetComponent<EasyComponentsGetter>());
        }
    }

    public GameObject getGOFromArena(int indexArena, int indexObjet)
    {
        return getter[indexArena].getGameObject(indexObjet);
    }
}
