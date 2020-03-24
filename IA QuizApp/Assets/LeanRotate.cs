using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanRotate : MonoBehaviour
{
    Touch touch;
    Vector2 touchPosition;
    Quaternion rotationY;

    float rotateSpeed = 0.1f;
    
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.collider.name == "Scatterplot")
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeed, 0f);
                        transform.rotation = rotationY * transform.rotation;
                    }
                }
            }
        }
    }
}
