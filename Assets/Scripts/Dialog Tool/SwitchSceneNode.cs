using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(350)]
[NodeTint("#c242f5")]
public class SwitchSceneNode : DialogNode
{
    [Input(ShowBackingValue.Never)] public string previous;
    [Output(ShowBackingValue.Never)] public string next;

    public string sceneName;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();
    }

    public override void Activate()
    {
        GameManager.instance.SetGameStateExploration();
        W_SceneManager.instance.SwitchScene(sceneName);
    }

    public override bool Update()
    {
        return true;
    }

    public override DialogNode GetNextNode()
    {
        return null;
    }
}
