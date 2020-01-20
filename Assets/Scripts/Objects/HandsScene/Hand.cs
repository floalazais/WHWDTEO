using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] protected Image _iconUI = null;

    // Start is called before the first frame update
    void Start()
    {
        _iconUI.enabled = false;
    }

    public bool Interact()
    {
        _iconUI.enabled = false;
        return true;
    }

    public void SetNearPlayerMode()
    {
        _iconUI.enabled = true;
    }

    public void SetFarPlayerMode()
    {
        _iconUI.enabled = false;
    }
}
