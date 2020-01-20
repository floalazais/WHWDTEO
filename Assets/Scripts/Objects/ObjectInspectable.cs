using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInspectable : ObjectViewable
{
    [SerializeField] GameObject _partToFind = null;
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Transform _cameraLookAt;

    bool _inspected = false;

    Vector3 wallHitPosition;

    void Start()
    {
        Init();

        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        SetModePresent();
    }

    void Update()
    {
        if (gameObject.layer != Utils_Variables.LAYER_OBJECT_INTERACT || _inspected) return;

        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Linecast(_cameraFollow.position, _cameraFollow.position + (_cameraLookAt.position - _cameraFollow.position) * 5, out hitInfo))
        {
            if (hitInfo.collider.gameObject == _partToFind)
            {
                print("trouvé !");
                _inspected = true;
            }
        }
    }
}
