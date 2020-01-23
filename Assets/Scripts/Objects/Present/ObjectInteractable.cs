using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectInteractable : W_Object
{
    public enum InteractionTime { PAST, PRESENT, BOTH}

    //[SerializeField] protected Image _iconUI = null;
    [SerializeField] protected CanvasObject _canvas = null;

    public InteractionTime interactionTime = InteractionTime.BOTH;

    [SerializeField] public bool interactable = true;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        SetModePresent();

        Init();
    }

    protected virtual void Init()
    {
        if(_canvas == null)
        {
            Debug.LogError("no canvas affected to " + name);
            return;
        }

        _canvas.SetFarPlayerMode();
    }

    public virtual void SetMediumPlayerMode()
    {
        _canvas.SetMediumPlayerMode();
    }

    public virtual void SetClosePlayerMode()
    {
        _canvas.SetClosePlayerMode();
    }

    public virtual void SetNearPlayerMode()
    {
        if (!interactable)
        {
            _canvas.SetFarPlayerMode();
            return;
        }

        _canvas.SetClosestPlayerMode();
    }

    public virtual void SetFarPlayerMode()
    {
        _canvas.SetFarPlayerMode();
    }

    public virtual void Interact()
    {
        _canvas.SetFarPlayerMode();
    }

    public override void SetModePresent()
    {
        base.SetModePresent();
        SetFarPlayerMode();

    }

    public override void SetModeMemory()
    {
        base.SetModeMemory();
    }

    public void SetInteractable(bool pInteractable)
    {
        interactable = pInteractable;
    }
}
