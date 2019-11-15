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
        if (GameManager.instance.state == Enums.E_GAMESTATE.INSPECTION) Manipulation();
    }

    void Manipulation()
    {
        rotationX += InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime;
        rotationY += InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime;
        //rotationX = Mathf.Clamp(rotationX, minX, maxX);
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        transform.localEulerAngles = new Vector3(rotationY, -rotationX, 0);
    }
}
