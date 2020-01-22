using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceElement : MonoBehaviour
{
    Text _choiceText;

    // Start is called before the first frame update
    void Start()
    {
        _choiceText = GetComponentInChildren<Text>();
    }

    public void EnableChoiceElement(string pText)
    {
        _choiceText.text = pText;
    }
}
