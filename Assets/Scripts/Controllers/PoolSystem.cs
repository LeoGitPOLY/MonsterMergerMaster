using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypePool
{
    MonsterPool,
    EggPool,
    CurrencyPool,
}

public delegate void FunctionDelegate();

public class PoolObject
{
    public GameObject obj;
    public EasyComponentsGetter getter;
    public FunctionDelegate function;

    public PoolObject(GameObject obj, EasyComponentsGetter getter)
    {
        this.obj = obj;
        this.getter = getter;
    }
}

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem instance;

    private List<PoolObject>[] pools;
    private Queue<int>[] queues;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }
    private void Start()
    {
        int numberPool = Enum.GetValues(typeof(TypePool)).Length;
        pools = new List<PoolObject>[numberPool];
        queues = new Queue<int>[numberPool];

        for (int i = 0; i < numberPool; i++)
        {
            pools[i] = new List<PoolObject>();
            queues[i] = new Queue<int>();
        }
    }

    public PoolObject InstantiatePool(GameObject prefab, TypePool type, Transform parent)
    {
        PoolObject poolObject = logicInstantiatePool(prefab, type, parent);
        return poolObject;
    }

    public void DestroyPool(GameObject prefab, float delay = 0)
    {
        StartCoroutine(delayDestroyPool(prefab, delay));
    }

    public List<MonsterData> getDataMonster()
    {
        List<MonsterData> mDatas = new List<MonsterData>();
        List<PoolObject> allMonsters = pools[(int)TypePool.MonsterPool];
        List<PoolObject> allEggs = pools[(int)TypePool.EggPool];
        Debug.LogError("GET DATA MONSTER");

        foreach (var item in allMonsters)
        {
            if (item.obj.activeSelf)
                mDatas.Add(item.obj.GetComponent<MonsterScript>().data);
        }

        foreach (var item in allEggs)
        {
            if (item.obj.activeSelf)
                mDatas.Add(item.obj.GetComponent<EggScript>().data);
        }
        Debug.LogError("Size: " + mDatas.Count);
        return mDatas;
    }

    // LOGIC METHODES:
    private (PoolObject, int) findPoolObject(TypePool type, GameObject prefab)
    {
        if (queues[(int)type].Count != 0)
        {
            int indexAvailable = queues[(int)type].Dequeue();
            return (pools[(int)type][indexAvailable], indexAvailable);
        }
        else
        {
            GameObject gameObject = Instantiate(prefab);
            EasyComponentsGetter getter = gameObject.GetComponent<EasyComponentsGetter>();
            PoolObject poolObj = new PoolObject(gameObject, getter);

            pools[(int)type].Add(poolObj);
            return (poolObj, pools[(int)type].Count - 1);
        }
    }

    private PoolObject logicInstantiatePool(GameObject prefab, TypePool type, Transform parent)
    {
        (PoolObject poolObject, int index) = findPoolObject(type, prefab);
        string name = type.ToString() + "_" + index.ToString();

        poolObject.obj.transform.parent = parent;
        poolObject.obj.SetActive(true);
        poolObject.obj.name = name;

        return poolObject;
    }
    private void logicDestroyPool(GameObject prefab)
    {
        string[] parts = prefab.name.Split('_');

        TypePool type = (TypePool)Enum.Parse(typeof(TypePool), parts[0]);
        int index = int.Parse(parts[1]);

        pools[(int)type][index].obj.SetActive(false);
        queues[(int)type].Enqueue(index);

        if (pools[(int)type][index].function != null)
        {
            pools[(int)type][index].function();
            pools[(int)type][index].function = null;
        }
    }

    private IEnumerator delayInstantiatePool(GameObject prefab, TypePool type, Transform parent, float delay)
    {
        yield return new WaitForSeconds(delay);
        logicInstantiatePool(prefab, type, parent);
    }
    private IEnumerator delayDestroyPool(GameObject prefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        logicDestroyPool(prefab);
    }
}
