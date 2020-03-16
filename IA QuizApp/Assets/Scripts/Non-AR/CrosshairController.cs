using UnityEngine;

public class CrosshairController : MonoBehaviour
{

    [SerializeField]
    [Range(50, 1000)]
    float speed;

    [SerializeField]
    FixedJoystick fixedJoystick;


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
    }
}