using UnityEngine;


[System.Serializable]
public class VersionData
{
    public EnumVersion s_enumVersion;

    public VersionData(VersionInstance version)
    {
        s_enumVersion = version.enumVersion;
    }
}