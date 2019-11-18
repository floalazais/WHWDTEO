using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    Collider _collider;
    MeshRenderer _meshRenderer;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }

    public void Display()
    {
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }

    public void Remove()
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }
}
