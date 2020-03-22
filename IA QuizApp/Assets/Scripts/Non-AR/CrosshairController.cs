using UnityEngine;
using Doozy.Engine.UI;

public class CrosshairController : MonoBehaviour
{
    [SerializeField]
    UICanvas canvas;

    [SerializeField]
    FixedJoystick fixedJoystick;

    [SerializeField]
    [Range(50, 1000)]
    float speed;

    Vector3 position;
    RectTransform rectTransform;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        position = new Vector3(fixedJoystick.Horizontal, fixedJoystick.Vertical, 0) * speed * Time.deltaTime;
        rectTransform.Translate(position);

        /*if(rectTransform.position.x > canvas.RectTransform.position.x)
        {
            Debug.Log("Crosshair: " + rectTransform.position + " Canvas: " + canvas.RectTransform.rect);
            rectTransform.Translate(-position);
        }*/

    }
}