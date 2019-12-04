using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to recognize special gestures with the touch pad and/or the joystick
/// </summary>
public class GestureRecognition : MonoBehaviour
{
    Vector2 _firstInputPosition, _endInputPosition;
    bool _isTouching = false;

    [SerializeField] float permissionZone = 0.5f;

    void StartGesture(OnTouch e)
    {
        _isTouching = true;
        _firstInputPosition = PS4Controller.Instance.firstPosition;
    }

    private void Update()
    {
        if (_isTouching) _endInputPosition = PS4Controller.Instance.firstPosition;
    }

    void CalculateTouchGestures(OnReleaseTouch e)
    {
        _isTouching = false;

        //create vector from the two points
        Vector2 currentSwipe = new Vector2(_endInputPosition.x - _firstInputPosition.x, _endInputPosition.y - _firstInputPosition.y);

        //normalize the 2d vector
        currentSwipe.Normalize();

        //swipe upwards
        if (currentSwipe.y > 0  && currentSwipe.x > -permissionZone && currentSwipe.x < permissionZone)
        {
            Debug.Log("up swipe");
        }
        //swipe down
        if (currentSwipe.y < 0 && currentSwipe.x > -permissionZone && currentSwipe.x < permissionZone)
        {
            Debug.Log("down swipe");
        }
        //swipe left
        if (currentSwipe.x < 0  && currentSwipe.y > -permissionZone && currentSwipe.y < permissionZone)
        {
            Debug.Log("left swipe");
        }
        //swipe right
        if (currentSwipe.x > 0 && currentSwipe.y > -permissionZone && currentSwipe.y < permissionZone)
        {
            Debug.Log("right swipe");
        }
    }
}
