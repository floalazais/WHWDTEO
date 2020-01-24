using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    [SerializeField] float sensX = 500.0f;
    [SerializeField] float sensY = 500.0f;

    Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == Utils_Variables.LAYER_OBJECT_INTERACT) Manipulation();
    }

    void Manipulation()
    {
        transform.Rotate(_camera.transform.rotation * new Vector3(InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime, -InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime, 0), Space.World);
    }
}