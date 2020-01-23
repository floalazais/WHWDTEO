using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasObject : MonoBehaviour
{
    [SerializeField] string _objectName = "OBJECT";
    [SerializeField] string _objectInteraction = "DO SOMETHING";
    Text _objectText = null;
    SpriteRenderer _interactionImg = null;
    SpriteRenderer _arrowImg = null;

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
                _objectText = child.GetComponent<Text>();
            }

            else if (child.name == Utils_Variables.OBJECT_CANVAS_INTERACTION)
            {
                _interactionImg = child.GetComponent<SpriteRenderer>();
            }

            else if (child.name == Utils_Variables.OBJECT_CANVAS_ARROW)
            {
                _arrowImg = child.GetComponent<SpriteRenderer>();
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
