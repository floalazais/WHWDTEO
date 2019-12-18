﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] RectTransform _descriptionObjectScreen = null;
    RectTransform _currentScreen = null;
    RectTransform _choicePanel = null;
    RectTransform _dialogPanel = null;

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
        _choicePanel = ChoicePanel.instance.GetComponent<RectTransform>();
        _dialogPanel = DialogPanel.instance.GetComponent<RectTransform>();

        _choicePanel.gameObject.SetActive(false);
        _dialogPanel.gameObject.SetActive(false);
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
        _currentScreen.gameObject.SetActive(false);
        _currentScreen = _choicePanel;
        _currentScreen.gameObject.SetActive(true);
    }

    public void OnDescriptionObject()
    {
        if(_currentScreen != null) _currentScreen.gameObject.SetActive(false);
        _currentScreen = _descriptionObjectScreen;
        _currentScreen.gameObject.SetActive(true);
    }

    public void RemoveScreen()
    {
        if (_currentScreen == null) return;

        _currentScreen.gameObject.SetActive(false);
        _currentScreen = null;
    }
}
