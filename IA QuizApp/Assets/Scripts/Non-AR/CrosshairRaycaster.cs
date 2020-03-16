using UnityEngine;

public class CrosshairRaycaster : MonoBehaviour
{
    [SerializeField]
    Color highlightColor;
    Color initialColor;

    RectTransform rectTransform;

    Ray ray;
    RaycastHit hitInfo;
    Vector3 rayOrigin;

    Collider hitObject;
    Material hitObjectMaterial;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rayOrigin = rectTransform.position;
        ray = Camera.main.ScreenPointToRay(rayOrigin);

        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            if (hitObject == null)
            {
                hitObject = hitInfo.collider;
                hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
                initialColor = hitObjectMaterial.color;
            }

            hitObjectMaterial.color = highlightColor;
        }
        else
        {
            if (hitObject != null)
            {
                hitObjectMaterial.color = initialColor;
                hitObject = null;
                hitObjectMaterial = null;
            }
        }
    }
}