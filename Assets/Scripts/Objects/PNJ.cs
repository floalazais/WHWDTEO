using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ : MonoBehaviour
{
    public string dialogName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PastManager.instance.state == Enums.E_PAST_STATE.PRESENT) CheckPlayerDistance();
    }

    protected void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, Controller.instance.transform.position);

        if (distance <= 2f)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DialogManager.instance.StartDialog(dialogName);
                GameManager.instance.SetGameStateNarration();
            }
        }
    }
}
