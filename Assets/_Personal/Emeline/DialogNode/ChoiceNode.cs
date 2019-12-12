using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public class Choice
{
    public string label;
    public bool locked;
}

[NodeTint("#dd444b")]
[NodeWidth(350)]
public class ChoiceNode : DialogNode {
    [Input(ShowBackingValue.Never)] public string previousDialog;
    [Output(dynamicPortList = true)] public Choice[] choicesArray;

    int selectedChoice = 0;

    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

    public override bool Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(choicesArray[0].label);
            selectedChoice = 0;
            return true;
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log(choicesArray[1].label);
            selectedChoice = 1;
            return true;
        } else if(Input.GetKeyDown(KeyCode.S)) {
            Debug.Log(choicesArray[2].label);
            selectedChoice = 2;
            return true;
        } else if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log(choicesArray[3].label);
            selectedChoice = 3;
            return true;
        } else {
            return false;
        }
    }

    public override DialogNode GetNextNode()
    {
        return GetPort("choicesArray " + selectedChoice).Connection.node as DialogNode;
    }
}