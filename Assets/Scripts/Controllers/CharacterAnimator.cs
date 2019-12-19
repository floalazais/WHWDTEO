using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] Enums.E_CHARACTER _name;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.AddListener<OnAnimatorEvent>(ChangeAnimation);
        EventsManager.Instance.AddListener<OnFacialEvent>(ChangeFacialExpression);
    }

    void ChangeAnimation(OnAnimatorEvent e)
    {
        if (e.character != _name) return;

        print(e.animatorParameter);
    }

    void ChangeFacialExpression(OnFacialEvent e)
    {
        if (e.character != _name) return;

        print(e.facialParameter);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnAnimatorEvent>(ChangeAnimation);
        EventsManager.Instance.RemoveListener<OnFacialEvent>(ChangeFacialExpression);
    }
}
