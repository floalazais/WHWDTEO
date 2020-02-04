using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    RectTransform _currentScreen = null;
    RectTransform _choicePanel = null;
    RectTransform _dialogPanel = null;
    RectTransform _manipulationPanel = null;
    RectTransform _letterPanel = null;
    [SerializeField] RectTransform _inspectionPanel = null;
    [SerializeField] Canvas _canvas = null;

    private void Awake()
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
        if(_canvas != null) _canvas.gameObject.SetActive(true);
        _choicePanel = ChoicePanel.instance.GetComponent<RectTransform>();
        _dialogPanel = DialogPanel.instance.GetComponent<RectTransform>();
        _letterPanel = LetterPanel.instance.GetComponent<RectTransform>();
        _manipulationPanel = ManipulationPanel.instance.GetComponent<RectTransform>();

        _choicePanel.gameObject.SetActive(false);
        _dialogPanel.gameObject.SetActive(false);
        _letterPanel.gameObject.SetActive(false);
        _manipulationPanel.gameObject.SetActive(false);
        _inspectionPanel.gameObject.SetActive(false);
    }

    public void OnStartDialog()
    {
        OnDialogScreen();
    }

    public void OnEndDialog()
    {
        if (_currentScreen != null) _currentScreen.gameObject.SetActive(false);
    }

    public void OnDialogScreen()
    {
        if(_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _dialogPanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void OnChoiceScreen()
    {
        if (_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _choicePanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void OnManipulationScreen()
    {
        if (_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _manipulationPanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void OnInspectionScreen()
    {
        if(_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _inspectionPanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void OnLetterPanel()
    {
        if (_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _letterPanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void RemoveScreen()
    {
        if (_currentScreen == null) return;

        _currentScreen.gameObject.SetActive(false);
        _currentScreen = null;
    }
}
