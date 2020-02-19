using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObject : MonoBehaviour
{
    [SerializeField] string _objectName = "OBJECT";
    [SerializeField] string _objectInteraction = "DO SOMETHING";
    TextMeshProUGUI _objectText = null;
    Image _interactionImg = null;
    Image _arrowImg = null;

    private void OnEnable()
    {
        AffectChildren();
        SetFarPlayerMode();
    }

    void AffectChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.name == Utils_Variables.OBJECT_CANVAS_TEXT)
            {
                _objectText = child.GetComponent<TextMeshProUGUI>();
            }

            else if (child.name == Utils_Variables.OBJECT_CANVAS_INTERACTION)
            {
                _interactionImg = child.GetComponent<Image>();
            }

            else if (child.name == Utils_Variables.OBJECT_CANVAS_ARROW)
            {
                _arrowImg = child.GetComponent<Image>();
            }
        }
    }

    public void SetFarPlayerMode()
    {
        _objectText.text = "";
        _arrowImg.enabled = false;
        _interactionImg.enabled = false;
    }

    public virtual void SetMediumPlayerMode()
    {
        _objectText.text = "";
        _arrowImg.enabled = true;
        _interactionImg.enabled = false;
    }

    public virtual void SetClosePlayerMode()
    {
        _objectText.text = _objectName;
        _arrowImg.enabled = true;
        _interactionImg.enabled = false;
    }

    public virtual void SetClosestPlayerMode()
    {
        _objectText.text = _objectInteraction;
        _arrowImg.enabled = true;
        _interactionImg.enabled = true;
    }
}
