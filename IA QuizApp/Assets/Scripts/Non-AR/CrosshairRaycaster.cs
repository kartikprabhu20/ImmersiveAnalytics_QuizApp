using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrosshairRaycaster : MonoBehaviour
{
    [SerializeField]
    GameObject overlayPrefab;
    [SerializeField]
    ScatterplotGenerator scatterplotData;
    [SerializeField]
    Text glyphDataText;
    [SerializeField]
    Color highlightColor;
    Color initialColor;

    RectTransform rectTransform;

    Ray ray;
    RaycastHit hitInfo;
    Vector3 rayOrigin;

    Collider hitObject;
    Material hitObjectMaterial;


    int layerMask;
    float hitDistance;

    bool isHighlighted;
    bool isScaled;
    string previousHit;
    string currentHit;
    int currentRaycastTimer;
    const int RaycastTimer = 125;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (SceneManager.GetActiveScene().name == "Non-AR Scene")
        {
            layerMask = LayerMask.GetMask("Points");
        }
        else
        {
            layerMask = ~(1 << LayerMask.NameToLayer("Planes"));
        }
        hitDistance = 15f;
    }

    void Update()
    {
        rayOrigin = rectTransform.position;
        ray = Camera.main.ScreenPointToRay(rayOrigin);

        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, hitDistance, layerMask))
        {
            if (hitObject == null)
            {
                hitObject = hitInfo.collider;
                hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
                initialColor = hitObjectMaterial.color;
            }

            isHighlighted = true;
            previousHit = currentHit;
            currentHit = hitObject.name;
            overlayPrefab.SetActive(true); // Add to AR
            overlayPrefab.GetComponent<Transform>().position = rayOrigin + new Vector3(100, 0, 0); // Add to AR
            DataReader.DataPoint glyphInfo = scatterplotData.GetGlyphData(short.Parse(hitObject.name)); // Add to AR
            glyphDataText.text = "  Point:" + hitObject.name + "\n" +
                                 "  x:  " + glyphInfo.X + "\n" +
                                 "  y:  " + glyphInfo.Y + "\n" +
                                 "  z:  " + glyphInfo.Z; // Add to AR
            if (currentRaycastTimer > RaycastTimer && !isScaled)
            {
                scatterplotData.AdjustGlyphScale(short.Parse(hitObject.name));
                isScaled = true;
            }

            if (previousHit != null && previousHit.Equals(currentHit))
            {
                currentRaycastTimer++;
            }

            if (!isScaled)
            {
                hitObjectMaterial.color = highlightColor;
            }
            GetComponent<AxisRaycast>().EnableRays();
            GetComponent<AxisRaycast>().UpdateLineRenderer(hitObject.transform); // Add to AR
        }
        else
        {
            isHighlighted = false;
            isScaled = false;
            currentRaycastTimer = 0;
            overlayPrefab.SetActive(false); // Add to AR
            GetComponent<AxisRaycast>().DisableRays(); // Add to AR
            if (hitObject != null)
            {
                if (!scatterplotData.GetColorStatus(short.Parse(hitObject.name)))
                {
                    scatterplotData.SetClusterColors();
                }
                hitObject = null;
                hitObjectMaterial = null;
            }

        }
    }
}