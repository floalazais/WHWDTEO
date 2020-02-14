using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    public static ChoicePanel instance { get; private set; }

    ChoiceElement[] _choicesElements;
    public bool isFadeDone = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if(_choicesElements == null) _choicesElements = GetComponentsInChildren<ChoiceElement>();
    }

    public void FillChoicesTextZone(string[] pChoicesArray)
    {
        for(int i = 0; i < _choicesElements.Length; i++)
        {
            if (i > pChoicesArray.Length - 1) DisableChoiceElement(i);
            else EnableChoiceElement(i, pChoicesArray);
        }
    }

    void EnableChoiceElement(int index, string[] pChoicesArray)
    {
        _choicesElements[index].gameObject.SetActive(true);
        _choicesElements[index].EnableChoiceElement(pChoicesArray[index]);

    }

    void DisableChoiceElement(int index)
    {
        _choicesElements[index].gameObject.SetActive(false);
    }

    public void OnEndChoice(int selectedChoice)
    {
        for(int i = 0; i < _choicesElements.Length; i++)
        {
            if (i == selectedChoice) continue;

            _choicesElements[i].FadeOut();
        }
    }
}
