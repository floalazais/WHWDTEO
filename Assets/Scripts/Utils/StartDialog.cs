using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogManager.instance.StartDialog("latinTest");
    }
}
