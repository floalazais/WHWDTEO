using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

abstract public class DialogNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

    abstract public bool Update();

    abstract public DialogNode GetNextNode();
}