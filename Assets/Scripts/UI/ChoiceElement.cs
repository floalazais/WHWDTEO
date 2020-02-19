using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceElement : MonoBehaviour
{
    [SerializeField] Text _choiceText = null;
    [SerializeField] Animator _choiceAnimator = null;

    // Start is called before the first frame update
    /*void Start()
    {
        _choiceText = GetComponentInChildren<Text>();
        _choiceAnimator = GetComponent<Animator>();
    }*/

    public void EnableChoiceElement(string pText)
    {
        _choiceAnimator.SetBool(Utils_Variables.IS_FADING, false);
        if (_choiceText == null) _choiceText = GetComponentInChildren<Text>();
        _choiceText.text = pText;
    }

    public void FadeOut()
    {
        _choiceAnimator.SetBool(Utils_Variables.IS_FADING, true);
        //_choiceAnimator.Play("FadeOut");
    }
}
