using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    //Main instance:
    public static PrefabHolder instance;

    [Header("Prefab Arena:")]
    [SerializeField] private List<GameObject> prefabArena;
    [SerializeField] private GameObject prefabMonster;

    private List<EasyComponentsGetter> getterArena;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        getterArena = new List<EasyComponentsGetter>();

        foreach (GameObject item in prefabArena)
        {
            getterArena.Add(item.GetComponent<EasyComponentsGetter>());
        }
    }

    public GameObject getGameObjectFromArena(int indexArena, int indexObjet)
    {
        return getterArena[indexArena].getGameObject(indexObjet);
    }
    public GameObject getMonsterPrefab()
    {
        return prefabMonster;
    }
}
