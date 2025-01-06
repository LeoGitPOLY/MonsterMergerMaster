using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(VariableHolder))]
public class custumInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VariableHolder holder = (VariableHolder)target;

        if (GUILayout.Button("Reimport Datas"))
        {
            holder.reimportAll();
        }
    }
}

public class IndexSCV
{
    public int iStartRow = -1;
    public int iStartCol = -1;

    public int iEndRow = -1;
    public int iEndCol = -1;

    public void printIndex()
    {
        Debug.Log(iStartCol + " " + iStartRow + "||" + iEndCol + " " + iEndRow);
    }
}

public class VariableHolder : MonoBehaviour
{
    [SerializeField] private TextAsset cvsStats;

    private List<string[]> rawDatas; // Colomn (j) - row(i) -> List de rangees
    private List<IndexSCV> indexsCSV;

    // currencyOfWhat_TYPELEVEl (LevelUp || MonsterLevel)
    private static double[] tSpawn_LU;
    private static int[] mGenerated_ML;

    private void Start()
    {
        reimportAll();
        print(getValueFromCell(indexsCSV[0].iEndCol, indexsCSV[0].iEndRow));
    }

    public void reimportAll()
    {
        importRawData();
        generateIndexCSV();
        importAllInGameValues();
    }


    private void importRawData()
    {
        rawDatas = new List<string[]>();
        string[] rowDatas = cvsStats.text.Split(new string[] { "\n" }, StringSplitOptions.None);

        for (int i = 0; i < rowDatas.Length; i++)
        {
            string[] cololmnDatas = rowDatas[i].Split(new string[] { ";" }, StringSplitOptions.None);
            rawDatas.Add(cololmnDatas);
        }
    }
    private void generateIndexCSV()
    {
        indexsCSV = new List<IndexSCV>();
        for (int i = 0; i < rawDatas.Count; i++)
        {
            string[] firstRow = rawDatas[i][0].Split(new string[] { ":" }, StringSplitOptions.None);

            if (firstRow[0] == "")
                // It's an empty line: Break of the loop
                break;
            else if (firstRow.Length == 1)
                // It's a title: Do nothing
                ;
            else if (firstRow.Length == 2)
            {
                IndexSCV index = new IndexSCV();

                index.iStartCol = Utils.CharUpperToInt(firstRow[0][0]);
                index.iStartRow = int.Parse(firstRow[0].Substring(1));

                index.iEndCol = Utils.CharUpperToInt(firstRow[1][0]);
                index.iEndRow = int.Parse(firstRow[1].Substring(1));

                indexsCSV.Add(index);
            }
        }
    }
    private void importAllInGameValues()
    {
    }
    // LOGIC METHODES:
    private string getValueFromCell(int colomn, int row)
    {
        string value = rawDatas[row - 1][colomn - 1];
        return value;
    }

    private void printRawData()
    {
        for (int i = 0; i < rawDatas.Count; i++)
        {
            string toPrint = "";

            for (int j = 0; j < rawDatas[i].Length; j++)
            {
                toPrint += rawDatas[i][j] + "#";
            }
            print(toPrint);
        }
    }

    // ASSESERS
    public static (int, int)[] Index { get => indexCSV; }


    private static (int, int)[] indexCSV;
    [SerializeField] public static int MAX_MONSTER_ARENA = 10;
}
