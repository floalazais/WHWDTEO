using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectInteractable : W_Object
{
    public enum InteractionTime { PAST, PRESENT, BOTH}

    [SerializeField] protected Text _text = null;

    public InteractionTime _interactionTime = InteractionTime.BOTH;

    [SerializeField] protected bool _interactable = true;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        SetModePresent();

        Init();
    }

    protected virtual void Init()
    {
        if(_text == null)
        {
            Debug.LogError("no text affected to " + name);
            return;
        }

        _text.enabled = false;
    }

    public virtual void SetNearPlayerMode()
    {
        if (!_interactable) return;

        _text.enabled = true;
    }

    public virtual void SetFarPlayerMode()
    {
        _text.enabled = false;
    }

    public virtual void Interact()
    {
        _text.enabled = false;
    }

    public override void SetModePresent()
    {
        base.SetModePresent();

    }

    public override void SetModeMemory()
    {
        base.SetModeMemory();
    }

    public void SetInteractable(bool pInteractable)
    {
        _interactable = pInteractable;
    }
}
