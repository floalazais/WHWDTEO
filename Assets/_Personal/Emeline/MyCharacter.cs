// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class MyCharacter : MonoBehaviour
{
    // The movement speed of this character
    public float moveSpeed = 3.0f;

    // The bullet speed
    public float bulletSpeed = 15.0f;

    // Assign a prefab to this in the inspector.
    // The prefab must have a Rigidbody component on it in order to work.
    public GameObject bulletPrefab;

    private CharacterController cc;
    private Vector3 moveVector;
    private bool fire;

    void Awake()
    {
        // Get the character controller
        cc = GetComponent<CharacterController>();
        EventsManager.Instance.AddListener<OnLeftStickMove>(Move);
        EventsManager.Instance.AddListener<OnRightStickMove>(Move);
    }

    void Update()
    {
        ProcessInput();
    }

    void Move(OnLeftStickMove e)
    {
        if (moveVector == e.move) return;

        moveVector = e.move;
    }

    void Move(OnRightStickMove e)
    {
        if (moveVector == e.move) return;

        moveVector = e.move;
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            //Vector3 moveVec = new Vector3(moveVector.x, 0, moveVector.y);

            cc.Move(moveVector * moveSpeed * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnLeftStickMove>(Move);
        EventsManager.Instance.RemoveListener<OnRightStickMove>(Move);
    }
}
