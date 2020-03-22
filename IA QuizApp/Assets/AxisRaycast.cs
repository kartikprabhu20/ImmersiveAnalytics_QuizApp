using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRaycast : MonoBehaviour
{
    [SerializeField]
    LineRenderer xLine;
    [SerializeField]
    LineRenderer yLine;
    [SerializeField]
    LineRenderer zLine;


    Ray xRay, yRay, zRay;
    RaycastHit hitInfo;

    int layerMask;
    float hitDistance;

    private void Awake()
    {
        xRay = new Ray(transform.position, Vector3.down);
        yRay = new Ray(transform.position, Vector3.left);
        zRay = new Ray(transform.position, Vector3.forward);

        layerMask = LayerMask.GetMask("Planes");
        hitDistance = 3f;
    }

    private void Update()
    {
        if(Physics.Raycast(xRay.origin, xRay.direction, out hitInfo, hitDistance, layerMask))
        {
            Debug.Log("Hitting X Axis");
            xLine.SetPosition(0, transform.position);
            xLine.SetPosition(1, hitInfo.point);
        }
        if (Physics.Raycast(yRay.origin, yRay.direction, out hitInfo, hitDistance, layerMask))
        {
            Debug.Log("Hitting Y Axis");
            yLine.SetPosition(0, transform.position);
            yLine.SetPosition(1, hitInfo.point);
        }
        if (Physics.Raycast(zRay.origin, zRay.direction, out hitInfo, hitDistance, layerMask))
        {
            Debug.Log("Hitting Z Axis");
            zLine.SetPosition(0, transform.position);
            zLine.SetPosition(1, hitInfo.point);
        }
    }
}
