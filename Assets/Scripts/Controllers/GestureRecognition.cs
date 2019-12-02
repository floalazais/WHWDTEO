﻿using System;
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

    float _newAngle, _oldAngle;
    bool _isRight = false;

    bool _isLSTurningRight = false;
    bool _isLSTurningLeft = false;

    bool _isRSTurningRight = false;
    bool _isRSTurningLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        //EventsManager.Instance.AddListener<OnTouch>(StartGesture);
        //EventsManager.Instance.AddListener<OnReleaseTouch>(CalculateTouchGestures);
        //EventsManager.Instance.AddListener<OnRightStickMove>(CalculateJoystickGestures);
    }

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

    void CalculateJoystickGestures(OnRightStickMove e)
    {
        Vector2 currentPosition = new Vector2(e.move.x, e.move.z);

        if (currentPosition.x > 0 && currentPosition.y > -permissionZone && currentPosition.y < permissionZone)
        {
            print("right");
            if (_isRSTurningRight || _isRSTurningLeft) print("360 no scope");
        }
        else if (currentPosition.x < 0 && currentPosition.y > -permissionZone && currentPosition.y < permissionZone)
        {
            print("left");
            if (_isRSTurningRight || _isRSTurningLeft) print("half-circle");
        }

        else if (currentPosition.y > 0 && currentPosition.x > -permissionZone && currentPosition.x < permissionZone)
        {
            print("up");
            if (_isRSTurningRight || _isRSTurningLeft) print("G");
        }

        else if (currentPosition.y < 0 && currentPosition.x > -permissionZone && currentPosition.x < permissionZone)
        {
            print("down");
            if (_isRSTurningRight || _isRSTurningLeft) print("Arc");
        }

        //print("origin : " + e.move.x + " , " + e.move.z);
        _oldAngle = _newAngle;
        _newAngle = Mathf.Atan2(e.move.x, e.move.z) * Mathf.Rad2Deg;
        //print(_newAngle);
        float lAngleDifference = _newAngle - _oldAngle;
        if (lAngleDifference > 180f) lAngleDifference -= 360f;
        if (lAngleDifference < -180f) lAngleDifference += 360f;

        if (lAngleDifference > 0)
        {
            _isRSTurningRight = true;
            _isRSTurningLeft = false;
        }
        else if (lAngleDifference < 0)
        {
            _isRSTurningRight = false;
            _isRSTurningLeft = true;
        }

        else
        {
            _isRSTurningRight = false;
            _isRSTurningLeft = false;
        }
    }

    public bool IsStickRolling(Enums.E_GAMEPAD_BUTTON pButton, Enums.E_ROLL_DIRECTION pRollDirection)
    {
        return pRollDirection == Enums.E_ROLL_DIRECTION.LEFT ? _isRSTurningLeft : _isRSTurningRight;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnTouch>(StartGesture);
        EventsManager.Instance.RemoveListener<OnReleaseTouch>(CalculateTouchGestures);
        EventsManager.Instance.RemoveListener<OnRightStickMove>(CalculateJoystickGestures);
    }
}
