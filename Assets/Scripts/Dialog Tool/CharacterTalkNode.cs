using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#ffaaaa")]
[NodeWidth(270)]
public class CharacterTalkNode : DialogNode {

    [Input(ShowBackingValue.Never)] public string previousDialog;
    [Output] public string nextDialog;

    public Enums.E_CHARACTER characterName;
    public string dialogSound;
    public string animator;
    public string facialTrigger;
    [SerializeField][TextArea(5, 10)] string dialogLine = "";

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

    public override void Activate()
    {
        UIManager.instance.OnDialogScreen();
        DialogPanel.instance.FillTextZone(dialogLine);
        //SoundManager.instance.PlaySound(dialogSound);

        EventsManager.Instance.Raise(new OnAnimatorEvent(characterName, animator));
        EventsManager.Instance.Raise(new OnFacialEvent(characterName, facialTrigger));
    }

    public override bool Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) return true;
        
        return false;
    }

    public override DialogNode GetNextNode()
    {
        return GetOutputPort("nextDialog").Connection.node as DialogNode;
    }
}