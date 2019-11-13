using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    Collider _collider;
    MeshRenderer _meshRenderer;

    private void Awake()
    {
        EventsManager.Instance.AddListener<ONR2ButtonDown>(Display);
        EventsManager.Instance.AddListener<ONR2ButtonUp>(Remove);
    }

    void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }

    void Display(ONR2ButtonDown e)
    {
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }

    void Remove(ONR2ButtonUp e)
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }
}
