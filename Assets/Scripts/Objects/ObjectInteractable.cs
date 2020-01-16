using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteractable : W_Object
{
    [SerializeField] protected Text _text = null;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        if(_text == null)
        {
            Debug.LogError("no text afefcted to " + name);
            return;
        }

        _text.enabled = false;
    }

    public virtual void SetNearPlayerMode()
    {
        _text.enabled = true;
    }

    public virtual void SetFarPlayerMode()
    {
        _text.enabled = false;
    }

    public virtual void Interact()
    {

    }

    public override void SetModePresent()
    {
        base.SetModePresent();
    }

    public override void SetModeMemory()
    {
        base.SetModeMemory();
    }
}
