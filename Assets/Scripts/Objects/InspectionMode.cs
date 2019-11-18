using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionMode : MonoBehaviour
{
    Camera _camera;
    public Transform objectViewTransform;

    public static InspectionMode instance { get { return _instance; } }
    static InspectionMode _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("ALREADY INSTANCE CREATED " + name);
            Destroy(_instance);
        }

        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }
}
