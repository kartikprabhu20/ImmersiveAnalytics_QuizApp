using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterplotManager : MonoBehaviour
{
    public GameObject glyph;
    public GameObject points;

    void Start()
    {
        for(int i=0; i<100; i++)
        {
            float randX = Random.Range(0, 100) * 0.01f;
            float randY = Random.Range(0, 100) * 0.01f;
            float randZ = Random.Range(0, 100) * 0.01f;
            Debug.Log(randX + " " + randY + " " + randZ);
            GameObject instance = Instantiate(glyph, points.transform);
            instance.transform.localPosition = new Vector3(randX, randY, randZ);
        }
    }

}
