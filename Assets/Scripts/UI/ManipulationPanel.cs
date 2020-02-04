using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulationPanel : MonoBehaviour
{
    public static ManipulationPanel instance { get; private set; }

    [Header("Sprite UI")]
    [SerializeField] Sprite swipeRightImg;
    [SerializeField] Sprite holdL2Img;
    [SerializeField] Sprite holdR2Img;
    [SerializeField] Sprite holdSquareImg;
    [SerializeField] Sprite rollRightImg;
    [SerializeField] Sprite rollLeftImg;
    [SerializeField] Sprite clickR1Img;

    [Header("Position UI")]
    [SerializeField] Image swipeImage;
    [SerializeField] Image clickImage;
    [SerializeField] Image holdImage;
    [SerializeField] Image rollImage;

    [Header("Animator UI")]
    [SerializeField] RuntimeAnimatorController R2Animator;
    [SerializeField] RuntimeAnimatorController L2Animator;
    [SerializeField] RuntimeAnimatorController SquareAnimator;
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
        DesactivateUI();
    }

    public void ActivateUI(Enums.E_GAMEPAD_BUTTON pButton, Enums.E_INTERACT_TYPE pType, Enums.E_ROLL_DIRECTION pRollDirection = Enums.E_ROLL_DIRECTION.NONE)
    {
        swipeImage.gameObject.SetActive(false);
        clickImage.gameObject.SetActive(false);
        rollImage.gameObject.SetActive(false);

        switch (pType)
        {
            case Enums.E_INTERACT_TYPE.CLICK:
                clickImage.gameObject.SetActive(true);
                clickImage.sprite = clickR1Img;
                break;

            case Enums.E_INTERACT_TYPE.HOLD:
                holdImage.gameObject.SetActive(true);
                if (pButton == Enums.E_GAMEPAD_BUTTON.L2_BUTTON)
                {
                    holdImage.sprite = holdL2Img;
                    holdImage.gameObject.GetComponent<Animator>().runtimeAnimatorController = L2Animator;
                }
                else if (pButton == Enums.E_GAMEPAD_BUTTON.R2_BUTTON)
                {
                    holdImage.sprite = holdR2Img;
                    holdImage.gameObject.GetComponent<Animator>().runtimeAnimatorController = R2Animator;
                }

                else if (pButton == Enums.E_GAMEPAD_BUTTON.SQUARE_BUTTON)
                {
                    holdImage.sprite = holdSquareImg;
                    holdImage.gameObject.GetComponent<Animator>().runtimeAnimatorController = SquareAnimator;
                }
                break;

            case Enums.E_INTERACT_TYPE.RELEASE_HOLD:
                holdImage.gameObject.SetActive(false);
                break;

            case Enums.E_INTERACT_TYPE.ROLL:
                rollImage.gameObject.SetActive(true);
                if (pRollDirection == Enums.E_ROLL_DIRECTION.LEFT) rollImage.sprite = rollLeftImg;
                else if(pRollDirection == Enums.E_ROLL_DIRECTION.RIGHT) rollImage.sprite = rollRightImg;
                break;

            case Enums.E_INTERACT_TYPE.SWIPE:
                swipeImage.gameObject.SetActive(true);
                swipeImage.sprite = swipeRightImg;
                break;
        }
    }

    public void StartHoldingAnimation()
    {
        holdImage.gameObject.GetComponent<Animator>().SetBool(Utils_Variables.IS_HOLDING, true);
    }

    public void StopHoldingAnimation()
    {
        holdImage.gameObject.GetComponent<Animator>().SetBool(Utils_Variables.IS_HOLDING, false);
    }

    public void DesactivateUI()
    {
        swipeImage.gameObject.SetActive(false);
        clickImage.gameObject.SetActive(false);
        holdImage.gameObject.SetActive(false);
        rollImage.gameObject.SetActive(false);

        StopHoldingAnimation();
    }
}
