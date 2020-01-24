using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    MeshRenderer _meshRenderer;
    Quaternion _rotation;
    public Vector3 scale;

    bool _isDisplaying = true;
    bool _isRemoving = true;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
        _rotation = transform.rotation;
    }

    private void Update()
    {
        if(transform.rotation != _rotation)
        {
            transform.rotation = _rotation;
        }

        if (_isDisplaying) {
            if (transform.localScale.x <= scale.x)
            {
                transform.localScale += new Vector3(.1f, .1f, .1f) * Time.deltaTime * 50f;
                return;
            }

            _isDisplaying = false;
            return;
        }

        if (_isRemoving)
        {
            if (transform.localScale.x >= 0)
            {
                transform.localScale -= new Vector3(.1f, .1f, .1f) * Time.deltaTime * 50f;
                return;
            }

            _meshRenderer.enabled = false;
            _isRemoving = false;
        }
    }

    public void Display()
    {
        _meshRenderer.enabled = true;
        _isDisplaying = true;
        _isRemoving = false;
    }

    public void Remove()
    {
        _isRemoving = true;
        _isDisplaying = false;
    }

}
