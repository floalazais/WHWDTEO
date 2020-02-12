using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    MeshRenderer _meshRenderer;
    Quaternion _rotation;
    public Vector3 scale;

    bool _isDisplaying = false;
    bool _isRemoving = false;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
        _rotation = transform.rotation;

        
    }

    private void Update()
    {
       // Shader.SetGlobalVector("_Position", transform.position);
       // Shader.SetGlobalVector("_Radius", transform.localScale);



        if (transform.rotation != _rotation)
        {
            transform.rotation = _rotation;
        }

        if (_isDisplaying) {
            if (transform.localScale.x <= scale.x)
            {
                transform.localScale += new Vector3(.1f, .1f, .1f) * Time.deltaTime * 50f;
                return;
            }

            transform.localScale = scale;
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

            transform.localScale = Vector3.zero;
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
