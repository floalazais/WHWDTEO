using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BeginNode : DialogNode {

    [Output] public string Start;

    public string dialogName;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

    public override void Activate()
    {

    }

    public override bool Update()
    {
        return true;
    }

    public override DialogNode GetNextNode()
    {
        return GetOutputPort("Start").Connection.node as DialogNode;
    }
}