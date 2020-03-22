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
        layerMask = LayerMask.GetMask("Planes");
        hitDistance = 3f;
    }

    private void SetRays(Transform m_transform)
    {
        xRay = new Ray(m_transform.position, Vector3.down);
        yRay = new Ray(m_transform.position, Vector3.left);
        zRay = new Ray(m_transform.position, Vector3.forward);

    }

    public void UpdateLineRenderer(Transform m_transform)
    {
        EnableRays();
        SetRays(m_transform);

        if (Physics.Raycast(xRay.origin, xRay.direction, out hitInfo, hitDistance, layerMask))
        {
            xLine.SetPosition(0, m_transform.position);
            xLine.SetPosition(1, hitInfo.point);
        }
        if (Physics.Raycast(yRay.origin, yRay.direction, out hitInfo, hitDistance, layerMask))
        {
            yLine.SetPosition(0, m_transform.position);
            yLine.SetPosition(1, hitInfo.point);
        }
        if (Physics.Raycast(zRay.origin, zRay.direction, out hitInfo, hitDistance, layerMask))
        {
            zLine.SetPosition(0, m_transform.position);
            zLine.SetPosition(1, hitInfo.point);
        }
    }

    private void EnableRays()
    {
        xLine.gameObject.SetActive(true);
        yLine.gameObject.SetActive(true);
        zLine.gameObject.SetActive(true);
    }

    public void DisableRays()
    {
        xLine.gameObject.SetActive(false);
        yLine.gameObject.SetActive(false);
        zLine.gameObject.SetActive(false);
    }
}
