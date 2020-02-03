using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulationPanel : MonoBehaviour
{
    public static ManipulationPanel instance { get; private set; }

    [SerializeField] Image swipeRightImg;
    [SerializeField] Image holdL2Img;
    [SerializeField] Image holdR2Img;
    [SerializeField] Image holdSquareImg;
    [SerializeField] Image rollRightImg;
    [SerializeField] Image rollLeftImg;
    [SerializeField] Image clickR1;

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
        swipeRightImg.gameObject.SetActive(false);
        holdL2Img.gameObject.SetActive(false);
        holdR2Img.gameObject.SetActive(false);
        rollRightImg.gameObject.SetActive(false);
        rollLeftImg.gameObject.SetActive(false);
        clickR1.gameObject.SetActive(false);
    }

    public void ActivateUI(Enums.E_GAMEPAD_BUTTON pButton, Enums.E_INTERACT_TYPE pType, Enums.E_ROLL_DIRECTION pRollDirection = Enums.E_ROLL_DIRECTION.NONE)
    {
        if(previousImage != null) previousImage.gameObject.SetActive(false);

        switch (pType)
        {
            case Enums.E_INTERACT_TYPE.CLICK:
                previousImage = clickR1;
                break;

            case Enums.E_INTERACT_TYPE.HOLD:
                if (pButton == Enums.E_GAMEPAD_BUTTON.L2_BUTTON) previousImage = holdL2Img;
                else if (pButton == Enums.E_GAMEPAD_BUTTON.R2_BUTTON) previousImage = holdR2Img;
                else if (pButton == Enums.E_GAMEPAD_BUTTON.SQUARE_BUTTON) previousImage = holdSquareImg;
                break;

            case Enums.E_INTERACT_TYPE.RELEASE_HOLD:
                previousImage = null;
                break;

            case Enums.E_INTERACT_TYPE.ROLL:
                if(pRollDirection == Enums.E_ROLL_DIRECTION.LEFT) previousImage = rollLeftImg;
                else if(pRollDirection == Enums.E_ROLL_DIRECTION.RIGHT) previousImage = rollRightImg;
                break;

            case Enums.E_INTERACT_TYPE.SWIPE:
                previousImage = swipeRightImg;
                break;
        }

        previousImage.gameObject.SetActive(true);
    }
}
