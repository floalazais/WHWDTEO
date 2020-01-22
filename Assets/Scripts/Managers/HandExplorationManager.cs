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

    [SerializeField] float timeBeforeEndCinematic = 1.0f;
    [SerializeField] float _interactionRadius = 1.5f;
    [SerializeField] float _closeRadius = 2.0f;
    [SerializeField] float _memoryZoneRadius = 3.0f;

    Hand _currentHand = null;

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

        for (int i = 0; i < _objectsArray.Count; i++)
        {
            _objectsArray[i].gameObject.SetActive(false);
        }

        SoundManager.instance.PlaySound(Utils_Variables.HOPE_GLITCH_SOUND);
        SetActiveHand();
    }

    void SortHandsByName()
    {
        _objectsArray = _objectsArray.OrderBy(go => go.name).ToList();
    }

    void SetActiveHand()
    {
        if (_index - 1 >= 0)
        {
            SoundManager.instance.PlaySound(Utils_Variables.DISPARITION_MAIN_SOUND);
            Invoke("DisableHand", 0.5f);
        }

        if (_index != 0) Invoke("ActiveHand", 3);
        else ActiveHand();
    }

    void ActiveHand()
    {
        _currentHand = _objectsArray[_index];
        _currentHand.gameObject.SetActive(true);
        SoundManager.instance.PlaySound(Utils_Variables.APPARITION_MAIN_SOUND);
    }

    //must disappear at a precise time of the sound
    void DisableHand()
    {
        _objectsArray[_index - 1].gameObject.SetActive(false);
    }

    void Update()
    {
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();
        if (GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION) CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        if (_currentHand == null) return;
        float distance = Vector3.Distance(_currentHand.transform.position, Controller.instance.transform.position);

        //If we're too far from the player
        if (distance > _memoryZoneRadius)
        {
            _currentHand.SetFarPlayerMode();
        }

        else
        {
            if (distance > _interactionRadius)
            {
                if (_objectNearPlayer == _currentHand)
                {
                    _objectNearPlayer.SetFarPlayerMode();
                    _objectNearPlayer = null;
                }

                //If we are close but can't interact
                if (distance <= _closeRadius)
                {
                    _currentHand.SetClosePlayerMode();
                    return;
                }

                //If we are a bit close but can't interact
                if (distance <= _memoryZoneRadius)
                {
                    _currentHand.SetMediumPlayerMode();
                    return;
                }
            }

            else
            {
                SetNearObject(_currentHand);
            }

        }
    }

    void SetInteractMode()
    {
        if (_objectNearPlayer == null) return;

        _objectNearPlayer.Interact();

        _index++;

        if (_index >= _objectsArray.Count)
        {
            Invoke("EndHandExploration", timeBeforeEndCinematic);
        }

        SetActiveHand();
    }

    public void SetNearObject(Hand pObject)
    {
        if (pObject == _objectNearPlayer) return;

        _objectNearPlayer = pObject;
        _objectNearPlayer.SetNearPlayerMode();
    }

    void EndHandExploration()
    {
        DialogManager.instance.StartDialog("toDialogTL");
        return;
    }
}
