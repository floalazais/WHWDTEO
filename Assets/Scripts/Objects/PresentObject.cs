using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentObject : MonoBehaviour
{
    MeshRenderer _meshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (PastManager.instance.state == Enums.E_PAST_STATE.PRESENT) SetModePresent();
        if (PastManager.instance.state == Enums.E_PAST_STATE.SEARCH_MODE) CheckPlayerDistance();
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, Controller.instance.transform.position);
        //Debug.DrawLine(MyCharacter.instance.transform.position, transform.position);

        if (distance > 3f)
        {
            //if (GameManager.instance.state != Enums.E_GAMESTATE.PLAY) return;
            SetModePresent();
        }

        else
        {
            SetModePast();
        }
    }

    void SetModePast()
    {
        _meshRenderer.enabled = false;
    }

    void SetModePresent()
    {
        _meshRenderer.enabled = true;
    }
}
