﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public class Choice
{
    public string label;
    public bool locked;
    public string condition;
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

    string[] GetChoicesText()
    {
        List<string> choicesTextList = new List<string>();

        for(int i = 0; i < choicesArray.Length; i++)
        {
            if(choicesArray[i].locked)
            {
                if (IsConditionValidated(choicesArray[i].condition)) choicesTextList.Add(choicesArray[i].label);
            }

            else choicesTextList.Add(choicesArray[i].label);
        }

        return choicesTextList.ToArray();
    }

    bool IsConditionValidated(string pCondition)
    {
        Dictionary<string, bool> variablesDictionary = (graph as DialogTool).variablesDictionary;

        if (variablesDictionary.ContainsKey(pCondition))
        {
            if (variablesDictionary[pCondition]) return true;
            else return false;
        }

        else return false;
    }

    public override void Activate()
    {
        UIManager.instance.OnChoiceScreen();
        ChoicePanel.instance.FillChoicesTextZone(GetChoicesText());
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