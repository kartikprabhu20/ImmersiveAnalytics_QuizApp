using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DataReader : MonoBehaviour
{
    public class DataPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Cluster { get; set; }
    }

    public DataPoint[] DataFrame { get; set; }

    public Dictionary<string, List<int>> ClusterMap = new Dictionary<string, List<int>>(); // cluster name as "key", cluster points as "values"

    public int RowCount { get; set; }
    public int ColumnCount { get; set; }

    char[] lineSeperator = { '\n' };
    char[] textSeperator = { ',' };


    public void LoadData(string fileName)
    {
        TextAsset fileContents = Resources.Load<TextAsset>(fileName);

        print("fileContents " + fileContents.text);

        string[] data = fileContents.text.Split(lineSeperator);
        string[] header = data[0].Split(textSeperator); // Get the header properties of the file

        RowCount = GetRowCount(data);
        ColumnCount = GetColumnCount(header);

        LoadDataFrame(data); // loads the contents from 'data' to 'DataFrame'

    }

    public void LoadData()
    {
        LoadData("Dataset/iris");// Default
    }

    private int GetRowCount(string[] data)
    {
        // Count ignoring the header and the last blank line
        return data.Length - 2;
    }

    private int GetColumnCount(string[] header)
    {
        return header.Length;
    }

    private void LoadDataFrame(string[] data)
    {
        int indexOffset = 1; // Used for ignoring the header

        DataFrame = new DataPoint[RowCount]; // Create a dataframe of RowCount size


        for (int i = 0; i < RowCount; i++)
        {
            string[] row = data[i + indexOffset].Split(textSeperator);
            int tokenIndex = 0; // Eg: {'2', '3', 'A', 'Info'} -> if tokenIndex = 2, then row[tokenIndex] is 'A', keeping track of tokens in row[]

            DataFrame[i] = new DataPoint();


            DataFrame[i].X = float.Parse(row[tokenIndex++]);
            DataFrame[i].Y = float.Parse(row[tokenIndex++]);
            DataFrame[i].Z = float.Parse(row[tokenIndex++]);

            DataFrame[i].Cluster = row[tokenIndex++];

            AddToCluster(DataFrame[i].Cluster, i);
        }
    }

    private void AddToCluster(string cluster, int value)
    {
        if (!ClusterMap.ContainsKey(cluster))
        {
            ClusterMap.Add(cluster, new List<int>());
        }

        ClusterMap[cluster].Add(value);
    }

}