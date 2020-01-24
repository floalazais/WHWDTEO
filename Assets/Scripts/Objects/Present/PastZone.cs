using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastZone : MonoBehaviour
{
    MeshRenderer _meshRenderer;
    Quaternion _rotation;
    Vector3 _scale;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _meshRenderer.enabled = false;
        _rotation = transform.rotation;
        _scale = transform.localScale;
    }

    private void Update()
    {
        if(transform.rotation != _rotation)
        {
            transform.rotation = _rotation;
        }    
    }

    public void Display()
    {
        _meshRenderer.enabled = true;

        transform.localScale = Vector3.zero;
        StartCoroutine(AnimationDisplay());
    }

    public void Remove()
    {
        StartCoroutine(AnimationRemove());
    }

    IEnumerator AnimationDisplay()
    {
        while(transform.localScale != _scale)
        {
            transform.localScale += new Vector3(.1f, .1f, .1f);
            yield return new WaitForSeconds(.015f);
        }
    }

    IEnumerator AnimationRemove()
    {
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale -= new Vector3(.1f, .1f, .1f);
            yield return new WaitForSeconds(.01f);
        }

        _meshRenderer.enabled = false;
    }
}
