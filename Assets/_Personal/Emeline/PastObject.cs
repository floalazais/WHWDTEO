using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastObject : MonoBehaviour
{
    MeshRenderer _meshRenderer = null;
    Collider _collider = null;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _meshRenderer.enabled = false;
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(name);
        if (!other.GetComponent<SphereCollider>()) return;
        _meshRenderer.enabled = true;
        _collider.isTrigger = false;
    }

    private void OnTriggerExit(Collider other)
    {
        print(name);
        if (!other.GetComponent<SphereCollider>()) return;
        _meshRenderer.enabled = false;
        _collider.isTrigger = true;
    }
}
