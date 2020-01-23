using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
    }

    public void Display()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = true;
    }

    public void Remove()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }
}
