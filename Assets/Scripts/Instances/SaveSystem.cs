using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //VERSION:
    public static void saveVersion(VersionInstance versionIns)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/version.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        VersionData data = new VersionData(versionIns);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static VersionData loadVersion()
    {
        string path = Application.persistentDataPath + "/version.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VersionData score = formatter.Deserialize(stream) as VersionData;
            stream.Close();

            Debug.Log("Save file found in " + path);
            return score;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //SCORE:
    public static void saveScore(ScoreInstance scoreIns)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/score.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData(scoreIns);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static ScoreData loadScore()
    {
        string path = Application.persistentDataPath + "/score.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData score = formatter.Deserialize(stream) as ScoreData;
            stream.Close();

            return score;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}