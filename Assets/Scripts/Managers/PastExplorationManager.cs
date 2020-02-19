using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PastExplorationManager : MonoBehaviour
{
    public static PastExplorationManager instance { get; private set; }

    List<Plush> _objectsArray = new List<Plush>();
    [SerializeField] Transform[] _plushSpawnPoints;
    Dictionary<Transform, Plush> _spawnPointsAndPlushes;
    Plush _objectNearPlayer;

    [SerializeField] Camera _objectCamera;

    bool _success = false;

    [SerializeField] float timeBeforeEndCinematic = 1.0f;
    [SerializeField] float _interactionRadius = 1.5f;
    [SerializeField] float _closeRadius = 2.0f;
    [SerializeField] float _memoryZoneRadius = 3.0f;

    [SerializeField] float _timer = 60f;
    [SerializeField] float _swapPeriod = 10.0f;
    float _swapTimer;

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

        _spawnPointsAndPlushes = new Dictionary<Transform, Plush>();
        for (int i = 0; i < _plushSpawnPoints.Length; i++)
        {
            _spawnPointsAndPlushes.Add(_plushSpawnPoints[i], null);
        }

        _swapTimer = _swapPeriod;
        ChangePlushesPositions();

        DialogManager.instance.StartDialog("beginPast");
        SoundManager.instance.PlaySound(Utils_Variables.PLAY_MUSIC_PAST_SOUND);

        DialogManager.instance._dialogGraph.variablesDictionary.Add("foundAllPlushes", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == Enums.E_GAMESTATE.NARRATION) return;

        if (InputManager.instance.IsButtonReleased(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) GoToPreviousState();
        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) SetInteractMode();

        if (GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION) CheckPlayerDistance();

        _swapTimer -= Time.deltaTime;
        if (_swapTimer <= 0.0f)
        {
            _swapTimer = _swapPeriod;
            ChangePlushesPositions();
        }

        CheckEndExploration();

        if (_timer <= 0.0f && GameManager.instance.state == Enums.E_GAMESTATE.EXPLORATION)
        {
            DialogManager.instance.StartDialog("toHandTL");
        }
    }

    void ChangePlushesPositions()
    {
        for (int i = 0; i < _plushSpawnPoints.Length; i++)
        {
            Plush currentLocationPlush = _spawnPointsAndPlushes[_plushSpawnPoints[i]];
            if (currentLocationPlush != null)
            {
                if (currentLocationPlush != _objectNearPlayer || GameManager.instance.state == Enums.E_GAMESTATE.MANIPULATION)
                {
                    _spawnPointsAndPlushes[_plushSpawnPoints[i]] = null;
                }
            }
        }

        for (int i = 0; i < _objectsArray.Count; i++)
        {
            if (_objectsArray[i].inspected) continue;
            if (_objectsArray[i] == _objectNearPlayer && GameManager.instance.state == Enums.E_GAMESTATE.MANIPULATION) continue;

            int newPositionIndex = Random.Range(0, _plushSpawnPoints.Length);
            while (true)
            {
                if (_spawnPointsAndPlushes[_plushSpawnPoints[newPositionIndex]] == null)
                {
                    _objectsArray[i].SetNewPosition(_plushSpawnPoints[newPositionIndex].position);
                    _spawnPointsAndPlushes[_plushSpawnPoints[newPositionIndex]] = _objectsArray[i];
                    break;
                }
                else
                {
                    newPositionIndex++;
                    newPositionIndex = newPositionIndex % _plushSpawnPoints.Length;
                }
            }
        }
    }

    void CheckEndExploration()
    {
        if (_success || _timer <= 0.0f) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0.0f) return;

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
            _success = true;
            Invoke("EndPlushExploration", timeBeforeEndCinematic);
        }
    }

    void EndPlushExploration()
    {
        DialogManager.instance._dialogGraph.variablesDictionary["foundAllPlushes"] = true;
        DialogManager.instance.StartDialog("toHandTL");
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
                continue;
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
                        continue;
                    }

                    //If we are a bit close but can't interact
                    if (distance <= _memoryZoneRadius)
                    {
                        lObject.SetMediumPlayerMode();
                        continue;
                    }

                    //lObjectInteractable.SetFarPlayerMode();
                }

                else
                {
                    if (distance <= lShortestDistance)
                    {
                        lShortestDistance = distance;
                        if (lNearestObject != null) lNearestObject.SetClosePlayerMode();
                        lNearestObject = lObject;
                    }
                    else {
                        lObject.SetClosePlayerMode();
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
        UIManager.instance.OnInspectionScreen();

        _objectNearPlayer.Interact();
    }

    void GoToPreviousState()
    {
        UIManager.instance.RemoveScreen();
        PutNearObject();
        GameManager.instance.SetGameStateExploration();
        if (_objectNearPlayer.inspected)
        {
            _objectNearPlayer.enabled = false;
            _objectNearPlayer.gameObject.AddComponent<Rigidbody>();
            SoundManager.instance.PlaySound(_objectNearPlayer.soundEventMusicBoxNotePut.Id);
            _objectCamera.fieldOfView = 57.5f;
            _objectNearPlayer = null;
        }
    }

    void PutNearObject()
    {
        _objectNearPlayer.SetNearPlayerMode();
        _objectNearPlayer.Put();
    }

    public void SetNearObject(Plush pObject)
    {
        if (pObject == _objectNearPlayer) return;
        
        _objectNearPlayer = pObject;
        _objectNearPlayer.SetNearPlayerMode();
    }
}
