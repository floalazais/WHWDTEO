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

            float distance = Vector3.Distance(lObject.transform.position, Controller.instance.transform.position);

            if (distance > _interactionRadius)
            {
                if (_objectNearPlayer == lObject)
                {
                    _objectNearPlayer.SetFarPlayerMode();
                    _objectNearPlayer = null;
                }
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
