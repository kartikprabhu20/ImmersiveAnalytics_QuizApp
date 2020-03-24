using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScatterplotGenerator : MonoBehaviour
{
    public GameObject defaultGlyph; // 3D model to generate as a glyph
    public GameObject graphGen;
    public GameObject pointsHolder; // Empty Parent to hold all the points, to avoid clutter

    [SerializeField]
    DataReader reader;

    DataReader.DataPoint[] dataPoints;

    public List<GameObject> glyphList = new List<GameObject>();

    Dictionary<string, int> clusterColorMap = new Dictionary<string, int>();
    int colorMapCounter = 0;

    Color[] clusterColors = { Color.magenta, Color.red, Color.gray, Color.yellow };

    int rowIndex = 0;
    int rowCount, columnCount;

    float xMin, yMin, zMin;
    float xMax, yMax, zMax;

    float plotscale = 0f;

    string fileName = "";

    public void reInit(string datasetName)
    {

        //}

        //public IEnumerator reInit(string datasetName)
        //{
        Debug.Log("Count:" + glyphList.Count);

        glyphList.Clear();

        if (dataPoints != null)
            Array.Clear(dataPoints, 0, dataPoints.Length);
        clusterColorMap.Clear();

        int i = 0;
        foreach (Transform child in pointsHolder.transform)
        {
            Debug.Log("Delete:" + child.name);
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);

            //if (i % 5 == 0)
            //{
            //    yield return new WaitForSeconds(.000000001f);
            //}
            i++;
        }
        initPlot(datasetName);
    }


    //PlotLoader plotLoadIndicator;

    public void initPlot(string fileName)
    {
        this.fileName = fileName;
        //plotLoadIndicator = plotLoad;
        reader.LoadData(fileName);
        dataPoints = reader.DataFrame;
        rowCount = reader.RowCount;
        columnCount = reader.ColumnCount;

        foreach (string clusterName in reader.ClusterMap.Keys)
        {
            clusterColorMap.Add(clusterName, colorMapCounter++);
        }

        StartCoroutine(createScatterplot());
    }

    public void assignScatterPlot(GameObject graphGenClone)
    {
        graphGen = graphGenClone;
        pointsHolder = graphGen.transform.Find("Points").gameObject;
    }

    IEnumerator createScatterplot()
    {

        float[] minValues = getMinValues();
        float[] maxValues = getMaxValues();

        setMinMax(minValues, maxValues);
        Vector3 objectPosition;
        GameObject glyph;

        int i = 0;
        foreach (DataReader.DataPoint point in dataPoints)
        {
            float plotOffset = 0.025f; // value offset to make sure point (0, 0) is not at the origin but a little away from it

            // normalize each data point & add offset
            float x = ((point.X - xMin) / (xMax - xMin)) + plotOffset;
            float y = ((point.Y - yMin) / (yMax - yMin)) + plotOffset;
            float z = ((point.Z - zMin) / (zMax - zMin)) + plotOffset;


            bool colorFlag = point.X == xMin || point.Y == yMin || point.Z == zMin || point.X == xMax || point.Y == yMax || point.Z == zMax;


            plotscale = graphGen.transform.localScale.x;
            Vector3 offsetToZeroCoordinate = new Vector3(-0.5f * plotscale, 0f, -0.5f * plotscale); //(-0.5,0,-0.5) is ZeroCordinates for plane
            objectPosition = (new Vector3(x, y, z)) * plotscale + pointsHolder.transform.position;// + offsetToZeroCoordinate;

            //yield return new WaitForSeconds(.000000001f);
            glyph = Instantiate(defaultGlyph, objectPosition, Quaternion.identity);
            //GlyphPoint glyphPoint = glyph.GetComponent("GlyphPoint") as GlyphPoint;
            glyph.transform.localScale *= plotscale; // Scale of GraphGen 

            glyph.name = fileName + i.ToString();
            //Debug.Log("Glyph: " + glyph.name + " " + glyph.transform.localScale);
            glyph.transform.rotation = pointsHolder.transform.rotation;
            glyph.transform.parent = pointsHolder.transform;

            if (clusterColorMap[point.Cluster] < clusterColors.Length)
            {
                glyph.GetComponent<Renderer>().material.color = clusterColors[clusterColorMap[point.Cluster]];
            }
            else
            {
                glyph.GetComponent<Renderer>().material.color = Color.white;
            }

            glyphList.Add(glyph);

            if (i % 5 == 0)
            {
                yield return new WaitForSeconds(.000000001f);
            }
            i++;
        }
        //plotLoadIndicator.callback();
    }

    float[] getMinValues()
    {
        float[] minValues = new float[3]; // { 0: x, 1: y, 2: z}

        minValues[0] = dataPoints[0].X;
        minValues[1] = dataPoints[0].Y;
        minValues[2] = dataPoints[0].Z;

        foreach (DataReader.DataPoint point in dataPoints)
        {
            if(point.X < minValues[0])
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
        //Debug.Log("getGlyphList: " + glyphList.Count);

        return glyphList;
    }

    public GameObject getGraphGen()
    {
        //Debug.Log("getGlyphList: " + glyphList.Count);

        return graphGen;
    }
}
