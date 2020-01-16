using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulation : MonoBehaviour
{
    [SerializeField] float sensX = 500.0f;
    [SerializeField] float sensY = 500.0f;

    // Update is called once per frame
    void Update()
    {
        if (PastManager.instance.state == Enums.E_LEVEL_STATE.INTERACT) Manipulation();
    }

    void Manipulation()
    {
        transform.Rotate(new Vector3(-InputManager.instance.rightVerticalAxis * sensY * Time.deltaTime, -InputManager.instance.rightHorizontalAxis * sensX * Time.deltaTime, 0), Space.World);
    }
}
