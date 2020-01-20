using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Object : MonoBehaviour
{
    [SerializeField]protected Material _memoryObjectMaterial = null;
    [SerializeField]protected Material _presentObjectMaterial = null;

    [SerializeField] protected MeshFilter _memoryObjectFilter = null;
    [SerializeField] protected MeshFilter _presentObjectFilter = null;

    [SerializeField] protected MeshRenderer _meshRenderer = null;
    protected MeshFilter _meshFilter = null;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        SetModePresent();
    }

    public virtual void SetModePresent()
    {
        if (_presentObjectMaterial == null && _presentObjectFilter == null)
        {
            _meshRenderer.enabled = false;
            return;
        }

        _meshFilter = _presentObjectFilter;
        _meshRenderer.enabled = true;
        _meshRenderer.material = _presentObjectMaterial; 
    }

    public virtual void SetModeMemory()
    {
        if (_memoryObjectMaterial == null && _memoryObjectFilter == null)
        {
            _meshRenderer.enabled = false;
            return;
        }

        _meshFilter = _memoryObjectFilter;
        _meshRenderer.enabled = true;
        _meshRenderer.material = _memoryObjectMaterial;
    }
}
