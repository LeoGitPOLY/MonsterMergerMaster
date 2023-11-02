using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionInstance : MonoBehaviour
{
    public static VersionInstance instance = null;

    public EnumVersion enumVersion;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

    }
    private void Start()
    {
        enumVersion = EnumVersion.initialVersion_0_0;
    }

    public void Save()
    {
        SaveSystem.saveVersion(instance);
    }

    public EnumVersion Load()
    {
        VersionData data = SaveSystem.loadVersion();

        return data != null ? data.s_enumVersion : EnumVersion.newVersion;
    }
}
