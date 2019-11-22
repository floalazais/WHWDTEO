using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialInputs : MonoBehaviour
{
    float _buttonDownTime = 0;
    float _buttonUpTime = 0;
    int _spamCount = 0;

    [SerializeField] float _spamGap = 0.5f;

    // Start is called before the first frame update
    /*void Start()
    {
        EventsManager.Instance.AddListener<ONR2ButtonDown>(ButtonDown);
        EventsManager.Instance.AddListener<ONR2ButtonUp>(ButtonUp);
    }

    void ButtonDown(ONR2ButtonDown e)
    {
        _buttonDownTime = Time.fixedTime;
    }

    void ButtonUp(ONR2ButtonUp e)
    {
        _buttonUpTime = Time.fixedTime;
        if (_buttonUpTime - _buttonDownTime < _spamGap)
        {
            _spamCount++;
            GetSpamLenght();
        }

        else _spamCount = 0;
    }

    void GetSpamLenght()
    {
        if (_spamCount < 3) return;
        else if (_spamCount >= 3 && _spamCount < 15) print("spam");
        else print("spam long");
    }*/
}
