using UnityEngine;

public class SaveInstance : MonoBehaviour
{
    public static SaveInstance instance = null;

    [SerializeField] private bool isLoad;
    [SerializeField] private bool develloperMode;

    //Save by time
    [SerializeField] private float interpolationPeriod;
    private float time = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }
    private void Start()
    {
        loadScenePrincipal();
    }
    void LateUpdate()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = 0.0f;
            saveAll();
        }
    }
    private void OnApplicationQuit()
    {
        saveAll();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveAll();
        }
    }

    private void loadScenePrincipal()
    {
        //WILL NEED TO BE SAVED AND LOAD
        EnumVersion version = VersionInstance.instance.Load();
        print(version);

        if (isLoad)
        {
            //LOAD VERSION:
            ScoreInstance.instance.Load(version);
        }
        else
        {
            //LOAD NEW:
            loadEmpty();
        }

        //Ajout: si develloper alors CHANGE les stats
        if (develloperMode)
        {
            ScoreInstance.instance.loadDevelloperMode();
        }
    }

    private void loadEmpty()
    {
        ScoreInstance.instance.loadNew();
    }

    public void saveAll()
    {
        VersionInstance.instance.Save();
        ScoreInstance.instance.Save();


        print("saveALLL");
    }
    

}
