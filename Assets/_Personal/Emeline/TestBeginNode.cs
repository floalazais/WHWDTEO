using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestBeginNode : Node {

    [Output(ShowBackingValue.Always)] public float beginValue;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
        if (port.fieldName == "beginValue") return beginValue;
		else return null;
	}
}