using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandExplorationManager : MonoBehaviour
{
    public static HandExplorationManager instance { get; private set; }
    List<Hand> _objectsArray = new List<Hand>();
    Hand _objectNearPlayer;
    int _index = 0;

    [SerializeField] float _interactionRadius = 1.5f;

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
        _objectsArray = GameObject.FindObjectsOfType<Hand>().ToList();
        SortHandsByName();

        for(int i = 0; i < _objectsArray.Count; i++)
        {
            _objectsArray[i].gameObject.SetActive(false);
        }

        SetActiveHand();
    }

    void SortHandsByName()
    {
        _objectsArray = _objectsArray.OrderBy(go => go.name).ToList();
    }

    void SetActiveHand()
    {
        if (_index - 1 >= 0) _objectsArray[_index - 1].gameObject.SetActive(false);

        _objectsArray[_index].gameObject.SetActive(true);
    }

    void Update()
    {
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();
        if (GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION) CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        float lShortestDistance = _interactionRadius;
        Hand lNearestObject = null;

        for (int i = 0; i < _objectsArray.Count; i++)
        {
            Hand lObject = _objectsArray[i];

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

        if (_objectNearPlayer.Interact())
        {
            _index++;

            if(_index >= _objectsArray.Count)
            {
                DialogManager.instance.StartDialog("toDialogTL");
                return;
            }

            SetActiveHand();
        }
    }

    public void SetNearObject(Hand pObject)
    {
        if (pObject == _objectNearPlayer) return;

        _objectNearPlayer = pObject;
        _objectNearPlayer.SetNearPlayerMode();
    }
}
