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
    public string condition;
}

[NodeWidth(350)]
[NodeTint("#8888ff")]
public class ChoiceNode : DialogNode {
    [Input(ShowBackingValue.Never)] public string previousDialog;
    [Output(dynamicPortList = true)] public Choice[] choicesArray;

    int selectedChoice = 0;

    float _timerFade = 1f;
    float _timer = 0;
    bool _chosen = false;

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
        _chosen = false;

        UIManager.instance.OnChoiceScreen();
        ChoicePanel.instance.FillChoicesTextZone(GetChoicesText());
    }

    public override bool Update()
    {
        if(_chosen)
        {
            _timer += Time.deltaTime;
            if(_timer >= _timerFade)
            {
                _chosen = false;
                _timer = 0.0f;
                UIManager.instance.OnEndDialog();
                return true;
            }
            else
            {
                return false;
            }
        }

        if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.TRIANGLE_BUTTON)) {
            selectedChoice = 0;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        } else if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.SQUARE_BUTTON)) {
            selectedChoice = 1;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        } else if(InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.ROUND_BUTTON)) {
            selectedChoice = 2;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        } else if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.CROSS_BUTTON)) {
            selectedChoice = 3;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        } else if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.D_PAD_UP_BUTTON)) {
            selectedChoice = 4;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        } else if (InputManager.instance.IsButtonPressed(Enums.E_GAMEPAD_BUTTON.D_PAD_LEFT_BUTTON)) {
            selectedChoice = 5;
            ChoicePanel.instance.OnEndChoice(selectedChoice);
            _chosen = true;
        }
        return false;
    }

    public override DialogNode GetNextNode()
    {
        return GetPort("choicesArray " + selectedChoice).Connection.node as DialogNode;
    }
}