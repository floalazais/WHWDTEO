using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class TestGraph : NodeGraph
{
    public void GetFinalValue()
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] as TestEndNode) (nodes[i] as TestEndNode).DisplayValue();
        }
    }
}
