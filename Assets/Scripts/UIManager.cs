using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get { return _instance; } }
    static UIManager _instance;

    [SerializeField] RectTransform _descriptionObjectScreen = null;
    RectTransform _currentScreen = null;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentScreen = _descriptionObjectScreen;
    }

    public void OnDescriptionObject()
    {
        if(_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _descriptionObjectScreen;
        _currentScreen.gameObject.SetActive(true);
    }

    public void RemoveScreen()
    {
        _currentScreen.gameObject.SetActive(false);
        _currentScreen = null;
    }
}
