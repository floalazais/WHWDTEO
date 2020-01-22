using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLetters : ObjectInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        SetModePresent();

        Init();
    }

    public override void Interact()
    {
        base.Interact();
        SoundManager.instance.PlaySound(Utils_Variables.PRISE_BOITE_SOUND);
        print("box letters");
        UIManager.instance.OnLetterPanel();
    }
}
