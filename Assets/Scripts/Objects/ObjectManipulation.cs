using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    [SerializeField] float sensX = 500.0f;
    [SerializeField] float sensY = 500.0f;

    public bool stop = false;

    Camera _camera;
    [SerializeField] Camera _objectCamera;

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer != Utils_Variables.LAYER_OBJECT_INTERACT) return;
        if (!stop) Manipulation();
        else _objectCamera.fieldOfView = Mathf.Lerp(_objectCamera.fieldOfView, 30.0f, Time.deltaTime);
    }

    void Manipulation()
    {
        transform.Rotate(_camera.transform.rotation * new Vector3(InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime, -InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime, 0), Space.World);
        transform.Rotate(_camera.transform.rotation * new Vector3(Input.GetAxis("Mouse Y") * sensY * Time.deltaTime, -Input.GetAxis("Mouse X") * sensX * Time.deltaTime, 0), Space.World);
    }
}