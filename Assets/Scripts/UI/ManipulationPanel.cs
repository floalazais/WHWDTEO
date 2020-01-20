using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulationPanel : MonoBehaviour
{
    public static ManipulationPanel instance { get; private set; }

    [SerializeField] Image holdUI;
    [SerializeField] Image releaseHoldUI;
    [SerializeField] Image clickUI;
    [SerializeField] Image spamUI;
    [SerializeField] Image rollUI;
    [SerializeField] Image swipeUI;
    [SerializeField] Image moveUI;

    Image previousImage;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("already an instance of " + name);
            Destroy(instance);
        }

        instance = this;
    }

    private void Start()
    {
        holdUI.gameObject.SetActive(false);
        releaseHoldUI.gameObject.SetActive(false);
        clickUI.gameObject.SetActive(false);
        spamUI.gameObject.SetActive(false);
        rollUI.gameObject.SetActive(false);
        swipeUI.gameObject.SetActive(false);
        moveUI.gameObject.SetActive(false);
    }

    public void ActivateUI(Enums.E_GAMEPAD_BUTTON pButton, Enums.E_INTERACT_TYPE pType)
    {
        if(previousImage != null) previousImage.gameObject.SetActive(false);

        switch (pType)
        {
            case Enums.E_INTERACT_TYPE.CLICK:
                previousImage = clickUI;
                break;

            case Enums.E_INTERACT_TYPE.HOLD:
                previousImage = holdUI;
                break;

            case Enums.E_INTERACT_TYPE.MOVE:
                previousImage = moveUI;
                break;

            case Enums.E_INTERACT_TYPE.RELEASE_HOLD:
                previousImage = releaseHoldUI;
                break;

            case Enums.E_INTERACT_TYPE.ROLL:
                previousImage = rollUI;
                break;

            case Enums.E_INTERACT_TYPE.SPAM:
                previousImage = spamUI;
                break;

            case Enums.E_INTERACT_TYPE.SWIPE:
                previousImage = swipeUI;
                break;
        }

        previousImage.gameObject.SetActive(true);
    }
}
