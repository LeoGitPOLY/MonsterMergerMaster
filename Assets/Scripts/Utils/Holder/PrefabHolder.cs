using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    //Main instance:
    public static PrefabHolder instance;

    [Header("Prefab Arena:")]
    [SerializeField] private List<GameObject> prefabArena;
  
    [Header("Prefab Monster:")]
    [SerializeField] private GameObject prefabMonster;
    [SerializeField] private GameObject prefabEgg;
    [SerializeField] private List<GameObject> prefabCurrency;

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

    public GameObject getCurrencyFromArena(int indexArena)
    {
        return prefabCurrency[indexArena];
    }
    public GameObject getGameObjectFromArena(int indexArena, int indexObjet)
    {
        return getterArena[indexArena].getGameObject(indexObjet);
    }
    public GameObject getMonsterPrefab()
    {
        return prefabMonster;
    }
    public GameObject getEggPrefab()
    {
        return prefabEgg;
    }
}
