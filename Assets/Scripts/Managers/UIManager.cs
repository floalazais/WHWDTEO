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
        if (ChoicePanel.instance != null) _choicePanel = ChoicePanel.instance.GetComponent<RectTransform>();
        if (DialogPanel.instance != null) _dialogPanel = DialogPanel.instance.GetComponent<RectTransform>();
        if (LetterPanel.instance != null) _letterPanel = LetterPanel.instance.GetComponent<RectTransform>();
        if (ManipulationPanel.instance != null) _manipulationPanel = ManipulationPanel.instance.GetComponent<RectTransform>();

        if (_choicePanel != null) _choicePanel.gameObject.SetActive(false);
        if (_dialogPanel != null) _dialogPanel.gameObject.SetActive(false);
        if (_letterPanel != null) _letterPanel.gameObject.SetActive(false);
        if (_manipulationPanel != null) _manipulationPanel.gameObject.SetActive(false);
        if (_inspectionPanel != null) _inspectionPanel.gameObject.SetActive(false);
    }

    public void OnStartDialog()
    {
        OnDialogScreen();
    }

    public void OnEndDialog()
    {
        _dialogPanel.gameObject.SetActive(false);
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
        if (_currentScreen != null && _currentScreen != _dialogPanel) _currentScreen.gameObject.SetActive(false);
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
