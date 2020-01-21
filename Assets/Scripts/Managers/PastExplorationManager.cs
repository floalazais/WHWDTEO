using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PastExplorationManager : MonoBehaviour
{
    public static PastExplorationManager instance { get; private set; }

    List<Plush> _objectsArray = new List<Plush>();
    Plush _objectNearPlayer;

    [SerializeField] float _interactionRadius = 1.5f;
    [SerializeField] float _closeRadius = 2.0f;
    [SerializeField] float _memoryZoneRadius = 3.0f;

    [SerializeField] float _timer = 60f;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(instance);
        }

        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        _objectsArray = GameObject.FindObjectsOfType<Plush>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION) return;

        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) GoToPreviousState();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();

        if (GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION) CheckPlayerDistance();

        CheckEndExploration();
    }

    void CheckEndExploration()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) print("end explo lose");

        else
        {
            bool lSuccess = true;

            foreach (Plush plush in _objectsArray)
            {
                if (!plush.inspected)
                {
                    lSuccess = false;
                    break;
                }
            }

            if (lSuccess)
            {
                print("explo win");
                DialogManager.instance.StartDialog("toHandTL");
            }
        }
    }

    protected void CheckPlayerDistance()
    {
        float lShortestDistance = _interactionRadius;
        Plush lNearestObject = null;

        for (int i = 0; i < _objectsArray.Count; i++)
        {
            Plush lObject = _objectsArray[i];
            if (lObject.inspected)
            {
                lObject.SetFarPlayerMode();
                return;
            }

            float distance = Vector3.Distance(lObject.transform.position, Controller.instance.transform.position);

            //If we're too far from the player
            if (distance > _memoryZoneRadius)
            {
                lObject.SetFarPlayerMode();
            }

            //If we're not
            else
            {
                //If the object is too far from being interactable with player
                if (distance > _interactionRadius)
                {
                    //If the closest object is now too far
                    if (_objectNearPlayer == lObject)
                    {
                        _objectNearPlayer.SetFarPlayerMode();
                        _objectNearPlayer = null;
                    }

                    //If we are close but can't interact
                    if (distance <= _closeRadius)
                    {
                        lObject.SetClosePlayerMode();
                        return;
                    }

                    //If we are a bit close but can't interact
                    if (distance <= _memoryZoneRadius)
                    {
                        lObject.SetMediumPlayerMode();
                        return;
                    }

                    //lObjectInteractable.SetFarPlayerMode();
                }

                else
                {
                    if (distance <= lShortestDistance)
                    {
                        lShortestDistance = distance;
                        lNearestObject = lObject;
                    }
                }
            }

        }

        if (lNearestObject != null)
        {
            SetNearObject(lNearestObject);
        }
    }

    void SetInteractMode()
    {
        if (_objectNearPlayer == null) return;

        GameManager.instance.SetGameStateManipulation();

        _objectNearPlayer.Interact();
    }

    void GoToPreviousState()
    {
        PutNearObject();
        GameManager.instance.SetGameStateExploration();
    }

    void PutNearObject()
    {
        _objectNearPlayer.SetNearPlayerMode();
    }

    public void SetNearObject(Plush pObject)
    {
        if (pObject == _objectNearPlayer) return;

        _objectNearPlayer = pObject;
        _objectNearPlayer.SetNearPlayerMode();
    }
}
