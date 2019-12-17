using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    public static ChoicePanel instance { get; private set; }

    Text[] _choicesTextZones;

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
        _choicesTextZones = GetComponentsInChildren<Text>();
    }

    public void FillChoicesTextZone(string[] pChoicesArray)
    {
        for(int i = 0; i < _choicesTextZones.Length; i++)
        {
            if (i > pChoicesArray.Length - 1) _choicesTextZones[i].text = "";
            else _choicesTextZones[i].text = pChoicesArray[i];
        }
    }
}
