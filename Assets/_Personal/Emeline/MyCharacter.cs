// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class MyCharacter : MonoBehaviour
{
    // The movement speed of this character
    public float moveSpeed = 3.0f;

    public static MyCharacter instance { get { return _instance; } }
    static MyCharacter _instance;

    private CharacterController cc;
    private Vector3 moveVector;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;

        // Get the character controller
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
       if(GameManager.instance.state == Enums.E_GAMESTATE.PLAY) ProcessInput();
    }

    private void ProcessInput()
    {
        // Process movement
        if (InputManager.instance.rightHorizontalAxis != 0.0f || InputManager.instance.rightVerticalAxis != 0.0f)
        {
            Vector3 moveVec = new Vector3(InputManager.instance.rightHorizontalAxis, 0, InputManager.instance.rightVerticalAxis);

            cc.Move(moveVec * moveSpeed * Time.deltaTime);
        }
    }
}
