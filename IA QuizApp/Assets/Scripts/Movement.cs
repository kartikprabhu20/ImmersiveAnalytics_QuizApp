using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 50f;
    public Camera camera;

    public FixedJoystick fixedJoystick;

    Vector3 rayOrigin;
    RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<RectTransform>().Translate(new Vector3(fixedJoystick.Horizontal * speed * Time.deltaTime, 0, 0));
        gameObject.GetComponent<RectTransform>().Translate(new Vector3(0, fixedJoystick.Vertical * speed * Time.deltaTime, 0));
       

        rayOrigin = gameObject.GetComponent<RectTransform>().transform.position;
        //Debug.Log(gameObject.GetComponent<RectTransform>().position);
        Ray ray = camera.ScreenPointToRay(rayOrigin);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            Debug.Log(hitInfo.collider.name + ": " + ray.direction);
        }

    }
}