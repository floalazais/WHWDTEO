using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceElement : MonoBehaviour
{
    Text _choiceText = null;

    // Start is called before the first frame update
    void Start()
    {
        _choiceText = GetComponentInChildren<Text>();
    }

    public void EnableChoiceElement(string pText)
    {
        if(_choiceText == null) _choiceText = GetComponentInChildren<Text>();
        _choiceText.text = pText;
    }
}
