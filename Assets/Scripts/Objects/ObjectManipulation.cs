using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    [SerializeField] float minX = -360.0f;
    [SerializeField] float maxX = 360.0f;

    [SerializeField] float minY = -180.0f;
    [SerializeField] float maxY = 180.0f;

    [SerializeField] float sensX = 500.0f;
    [SerializeField] float sensY = 500.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (PastManager.instance.state == Enums.E_PAST_STATE.INTERACT) Manipulation();
    }

    void Manipulation()
    {
        //transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(rotationY, -rotationX, 0);
        rotationX += InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime;
        rotationY += InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime;
        //transform.Rotate(new Vector3(InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime, -InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime, 0));
        //rotationX = Mathf.Clamp(rotationX, minX, maxX);
        //rotationY = Mathf.Clamp(rotationY, minY, maxY);
        transform.localEulerAngles = new Vector3(rotationY, -rotationX, 0);
    }
}
