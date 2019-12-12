using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#ffaaaa")]
[NodeWidth(300)]
public class CharacterTalkNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previousDialog;
    [Output] public string nextDialog;

    [SerializeField][TextArea(10, 20)] string dialogLine = "";

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

    public override bool Update()
    {
        Debug.Log(dialogLine);
        return true;
    }

    public override DialogNode GetNextNode()
    {
        return GetOutputPort("nextDialog").Connection.node as DialogNode;
    }
}