using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public static DialogPanel instance { get; private set; }

    [SerializeField] Text _textZone;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
        //_textZone = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    /*void Start()
    {
        _textZone = GetComponentInChildren<Text>();
    }*/

    public void FillTextZone(string pText)
    {
        _textZone.text = pText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
