using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RayCastSelector : MonoBehaviour
{
    public float rayCasteRange = 50f;                                   // Distance in Unity units over which the player can fire
    public Transform rayShooterPosition;                                // Holds a reference to the end of ray shooter, marking the muzzle location of the shooter
    public Camera fpsCam;                                               // Holds a reference to the first person camera
    //public ARController controller;
    private GameObject scatterPlotPrefab;
    private GameObject overlayPrefab;
    TextMesh glyphDataText;
    private GameObject scatterPlotManager;
    private ScatterplotGenerator scatterplotGenerator;
    //private GameObject tooltips;
    //private GameObject mainToolTip;
    //private GameObject xyToolTip;
    //private GameObject yzToolTip;
    //private GameObject xzToolTip;

    private LineRenderer laserLine;                                     // Reference to the LineRenderer component which will display our laserline
    private Color previousGameObjectColor;
    private GameObject previousGameObject;
    int layerMask;

    void Start()
    {
        // Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer>();
        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();
        //Debug.Log("controller:" + controller == null);
        //graphGen = controller.plotPrefab;
        Debug.Log("GraphGen:" + scatterPlotPrefab == null);
        layerMask = ~LayerMask.GetMask("Planes");



        //tooltips = controller.plotPrefab.transform.Find("Tooltips").gameObject;
        //mainToolTip = tooltips.transform.Find("MainTooltip").gameObject;
        //xyToolTip = tooltips.transform.Find("XYTooltip").gameObject;
        //xzToolTip = tooltips.transform.Find("XZTooltip").gameObject;
        //yzToolTip = tooltips.transform.Find("YZTooltip").gameObject;

        //Debug.Log("toolTip:" + mainToolTip == null);
        //Debug.Log("text:" + mainToolTip.GetComponent<TextMeshPro>().text);

    }

    internal void restore()
    {
        overlayPrefab.SetActive(false);
        GetComponent<AxisRaycast>().DisableRays();
    }

    void Update()
    {
        // Create a vector at the center of our camera's viewport
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        if(overlayPrefab != null)
            overlayPrefab.transform.LookAt(fpsCam.transform);

        if (laserLine.enabled)
        {
            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, rayShooterPosition.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, rayCasteRange, layerMask))
            {
                GetComponent<AxisRaycast>().EnableRays();
                GetComponent<AxisRaycast>().UpdateLineRenderer(hit.collider.transform);

                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);
                //Debug.Log("color1:" + previousGameObjectColor);

                overlayPrefab.SetActive(true); // Add to AR
                //overlayPrefab.GetComponent<Transform>().position = rayOrigin + new Vector3(100, 0, 0); // Add to AR

                DataReader.DataPoint glyphInfo = scatterplotGenerator.GetGlyphData(Int32.Parse(hit.collider.gameObject.name)); // Add to AR
                glyphDataText.text = "  Point:" + hit.collider.gameObject.name + "\n" +
                                     "  x:  " + glyphInfo.X + "\n" +
                                     "  y:  " + glyphInfo.Y + "\n" +
                                     "  z:  " + glyphInfo.Z; // Add to AR

                if (previousGameObject != hit.collider.gameObject)
                {
                    resetGameObject();
                    previousGameObject = hit.collider.gameObject;
                    previousGameObjectColor = previousGameObject.GetComponent<MeshRenderer>().material.color;
                }
                previousGameObject.GetComponent<MeshRenderer>().material.color = new Color(230, 224, 209);


                //tooltips.SetActive(true);
                //mainToolTip.GetComponent<TextMeshPro>().SetText(previousGameObject.name);
                //xyToolTip.GetComponent<TextMeshPro>().SetText(previousGameObject.name);
                //yzToolTip.GetComponent<TextMeshPro>().SetText(previousGameObject.name);
                //xzToolTip.GetComponent<TextMeshPro>().SetText(previousGameObject.name);
                //Debug.Log("color2:"+previousGameObjectColor);
            }
            else
            {
                //tooltips.SetActive(false);
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                overlayPrefab.SetActive(false); 
                GetComponent<AxisRaycast>().DisableRays();
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * rayCasteRange));
                resetGameObject();

            }
        }
        else
        {    
           //Reset gameobject
           resetGameObject();
           if (previousGameObject != null)
                GetComponent<AxisRaycast>().UpdateLineRenderer(previousGameObject.transform);
        }

    }

    private void resetGameObject()
    {
        if (previousGameObject != null)
             previousGameObject.GetComponent<MeshRenderer>().material.color = previousGameObjectColor;
        

    }

    public void OnPointerDown()
    {
        //plotObject.GetComponent<MeshRenderer>().material.color = Color.yellow;

        laserLine.enabled = true;
    }

    public void OnPointerUp()
    {
        laserLine.enabled = false;
    }

    //internal void assignOverlay(GameObject overlay)
    //{
    //    overlayPrefab = overlay;
    //    glyphDataText = overlayPrefab.transform.Find("Background/DataText").gameObject.GetComponent<Text>();
    //}
    internal void assignScatterplotManager(GameObject scatterManager)
    {
        this.scatterPlotManager = scatterManager;
        Debug.Log("scatterPlotManager:" + scatterPlotManager.GetComponent<ScatterplotGenerator>().test());

        scatterplotGenerator = scatterPlotManager.GetComponent<ScatterplotGenerator>();

    }

    internal void assignScatterplot(GameObject scatterplot)
    {
        this.scatterPlotPrefab = scatterplot;
        overlayPrefab = scatterplot.transform.Find("DatapointOverlay").gameObject;
        glyphDataText = overlayPrefab.transform.Find("Text").GetComponent<TextMesh>();

    }
}
