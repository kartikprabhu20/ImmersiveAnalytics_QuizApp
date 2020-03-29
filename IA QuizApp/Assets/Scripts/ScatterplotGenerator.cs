using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterplotGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject glyphPrefab;
    [SerializeField]
    GameObject scatterplotPrefab;
    [SerializeField]
    GameObject pointsParent;
    [SerializeField]
    DataReader reader;

    Color defaultGlyphColor = Color.gray;
    Color highlightGlyphColor = Color.red;

    Color32[] colors = {  new Color32(255, 132, 0, 255), new Color32(66, 245, 66, 255), new Color32(250, 250, 0, 255),
                         new Color32(200, 0, 250, 255),new Color32(250, 0, 0, 255),new Color32(66, 66, 245, 255) }; //{Orange,Green, Yellow, Red, Purple,Blue

    DataReader.DataPoint[] dataPoints;

    public List<GameObject> glyphList = new List<GameObject>();
    bool[] glyphScaleList;


    float xMin, yMin, zMin;
    float xMax, yMax, zMax;

    float plotscale = 0f;
    float glyphScale = 1.5f;

    string fileName;

    public void reInit(string datasetName)
    {
        glyphList.Clear();

        if (dataPoints != null)
            Array.Clear(dataPoints, 0, dataPoints.Length);

        foreach (Transform child in pointsParent.transform)
        {
            Destroy(child.gameObject);
        }
        initPlot(datasetName);
    }


    public void initPlot(string fileName)
    {
        this.fileName = fileName;
        reader.LoadData(fileName);
        dataPoints = reader.DataFrame;

        StartCoroutine(createScatterplot());
    }

    public void assignScatterPlot(GameObject scatterplotPrefabClone)
    {
        scatterplotPrefab = scatterplotPrefabClone;
        pointsParent = scatterplotPrefab.transform.Find("Points").gameObject;
    }

    IEnumerator createScatterplot()
    {

        glyphScaleList = new bool[reader.RowCount];

        float[] minValues = getMinValues();
        float[] maxValues = getMaxValues();

        setMinMax(minValues, maxValues);
        Vector3 objectPosition;
        GameObject glyph;

        int i = 0;
        foreach (DataReader.DataPoint point in dataPoints)
        {
            float plotOffset = 0f;

            // normalize each data point & add offset
            float x = ((point.X - xMin) / (xMax - xMin)) + plotOffset;
            float y = ((point.Y - yMin) / (yMax - yMin)) + plotOffset;
            float z = ((point.Z - zMin) / (zMax - zMin)) + plotOffset;
           
            plotscale = scatterplotPrefab.transform.localScale.x;
            objectPosition = new Vector3(x, y, z);

            glyph = Instantiate(glyphPrefab, pointsParent.transform);
            glyph.name = i.ToString();
            glyph.layer = LayerMask.NameToLayer("Points");
            glyph.transform.localPosition = objectPosition;
            glyph.GetComponent<Renderer>().material.color = defaultGlyphColor;

            glyphList.Add(glyph);

            if (i % 5 == 0)
            {
                yield return new WaitForSeconds(.000000001f);
            }

            i++;
        }
        SetClusterColors();
    }

    float[] getMinValues()
    {
        float[] minValues = new float[3]; // { 0: x, 1: y, 2: z}

        minValues[0] = dataPoints[0].X;
        minValues[1] = dataPoints[0].Y;
        minValues[2] = dataPoints[0].Z;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            if (point.X < minValues[0])
            {
                minValues[0] = point.X;
            }
            if (point.Y < minValues[1])
            {
                minValues[1] = point.Y;
            }
            if (point.Z < minValues[2])
            {
                minValues[2] = point.Z;
            }
        }

        return minValues;
    }

    float[] getMaxValues()
    {
        float[] maxValues = new float[3]; // { 0: x, 1: y, 2: z}

        maxValues[0] = dataPoints[0].X;
        maxValues[1] = dataPoints[0].Y;
        maxValues[2] = dataPoints[0].Z;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            if (point.X > maxValues[0])
            {
                maxValues[0] = point.X;
            }
            if (point.Y > maxValues[1])
            {
                maxValues[1] = point.Y;
            }
            if (point.Z > maxValues[2])
            {
                maxValues[2] = point.Z;
            }
        }

        return maxValues;
    }

    void setMinMax(float[] minValues, float[] maxValues)
    {
        xMin = minValues[0];
        yMin = minValues[1];
        zMin = minValues[2];

        xMax = maxValues[0];
        yMax = maxValues[1];
        zMax = maxValues[2];
    }

    public List<GameObject> getGlyphList()
    {
        return glyphList;
    }

    public GameObject getscatterplotPrefab()
    {
        return scatterplotPrefab;
    }

    public DataReader.DataPoint GetGlyphData(int index)
    {
        return dataPoints[index];
    }

    public void AdjustGlyphScale(int index)
    {
        if (glyphScaleList[index])
        {
            glyphList[index].transform.localScale /= glyphScale;
            glyphList[index].GetComponent<Renderer>().material.color = defaultGlyphColor;
            glyphScaleList[index] = false;
        }
        else
        {
            glyphList[index].transform.localScale *= glyphScale;
            glyphList[index].GetComponent<Renderer>().material.color = highlightGlyphColor;
            glyphScaleList[index] = true;
        }
    }

    public bool GetColorStatus(int index)
    {
        return glyphScaleList[index];
    }

    public void SetClusterColors()
    {
        int index = 0;

        foreach(DataReader.DataPoint dataPoint in dataPoints)
        {
            switch (dataPoint.Cluster)
            {
                case "A": 
                    glyphList[index].GetComponent<Renderer>().material.color = colors[0];
                    break;

                case "B":
                    glyphList[index].GetComponent<Renderer>().material.color = colors[1];
                    break;

                case "C":
                    glyphList[index].GetComponent<Renderer>().material.color = colors[2];
                    break;

                case "D":
                    glyphList[index].GetComponent<Renderer>().material.color = colors[3];
                    break;

                case "E":
                    glyphList[index].GetComponent<Renderer>().material.color = colors[4];
                    break;

                default: 
                    glyphList[index].GetComponent<Renderer>().material.color = colors[5];
                    break;
            }
            index++;
        }
    }

    public string test()
    {
        return "tester";
    }

}
